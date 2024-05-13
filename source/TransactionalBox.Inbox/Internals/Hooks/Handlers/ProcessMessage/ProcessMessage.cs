using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using TransactionalBox.Base.EventHooks;
using TransactionalBox.Inbox.Contexts;
using TransactionalBox.Inbox.Internals.Assemblies.CompiledHandlers;
using TransactionalBox.Inbox.Internals.Assemblies.MessageTypes;
using TransactionalBox.Inbox.Internals.Deserialization;
using TransactionalBox.Inbox.Internals.Hooks.Events;
using TransactionalBox.Inbox.Internals.Storage;
using TransactionalBox.Internals;

namespace TransactionalBox.Inbox.Internals.Hooks.Handlers.ProcessMessage
{
    internal sealed class ProcessMessage : IEventHookHandler<AddedMessagesToInbox>
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly ICompiledInboxHandlers _compiledInboxHandlers;

        private readonly IInboxStorage _inboxStorage;

        private readonly IInboxDeserializer _deserializer;

        private readonly IInboxMessageTypes _inboxMessageTypes;

        private readonly ISystemClock _systemClock;

        private readonly IEventHookPublisher _eventHookPublisher;

        public ProcessMessage(
            IServiceProvider serviceProvider,
            ICompiledInboxHandlers compiledInboxHandlers,
            IInboxStorage inboxStorage,
            IInboxDeserializer deserializer,
            IInboxMessageTypes inboxMessageTypes,
            ISystemClock systemClock,
            IEventHookPublisher eventHookPublisher)
        {
            _serviceProvider = serviceProvider;
            _compiledInboxHandlers = compiledInboxHandlers;
            _inboxStorage = inboxStorage;
            _deserializer = deserializer;
            _inboxMessageTypes = inboxMessageTypes;
            _systemClock = systemClock;
            _eventHookPublisher = eventHookPublisher;
        }

        public async Task HandleAsync(IHookExecutionContext context, CancellationToken cancellationToken)
        {
            var nowUtc = _systemClock.UtcNow;

            //Semaphore (process all with max limit)

            InboxMessageStorage? inboxMessage;

            do
            {
                inboxMessage = await _inboxStorage.GetMessage(context.Id, context.Name, _systemClock.TimeProvider, TimeSpan.FromMinutes(1)).ConfigureAwait(false);

                if (inboxMessage is null)
                {
                    await _eventHookPublisher.PublishAsync<ProcessedMessageFromInbox>().ConfigureAwait(false);
                    return;
                }

                var messageTypeName = inboxMessage.Topic.Split('.')[1]; //TODO

                _inboxMessageTypes.DictionaryMessageTypes.TryGetValue(messageTypeName, out var type);

                // (Error case) TODO what when type does not exist

                Type handlerType = typeof(IInboxMessageHandler<>).MakeGenericType(type);

                var handler = _serviceProvider.GetRequiredService(handlerType);

                string metadataJson;
                string messageJson;

                using (var jsonDocument = JsonDocument.Parse(inboxMessage.Payload))
                {
                    var jsonRoot = jsonDocument.RootElement;

                    metadataJson = jsonRoot.GetProperty("Metadata").ToString();
                    messageJson = jsonRoot.GetProperty("Message").ToString();
                }

                var metadata = _deserializer.DeserializeMetadata(metadataJson);
                var message = _deserializer.DeserializeMessage(messageJson, type);

                IExecutionContext executionContext = new Contexts.ExecutionContext(metadata, cancellationToken);

                var compiledHandler = _compiledInboxHandlers.GetCompiledInboxHandler(type);

                await compiledHandler(handler, message, executionContext).ConfigureAwait(false);
            }
            while (inboxMessage is not null);
            //TODO now always when end because this hooks can long running

            await _eventHookPublisher.PublishAsync<ProcessedMessageFromInbox>().ConfigureAwait(false);
            //TODO create handler
        }
    }
}
