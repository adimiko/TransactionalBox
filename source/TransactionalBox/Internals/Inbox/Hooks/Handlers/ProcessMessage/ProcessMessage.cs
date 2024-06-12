using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using TransactionalBox.Internals.Inbox.Assemblies.CompiledHandlers;
using TransactionalBox.Internals.Inbox.Deserialization;
using TransactionalBox.Internals.Inbox.Hooks.Events;
using TransactionalBox.Internals;
using TransactionalBox.Internals.Inbox.Hooks.Handlers.ProcessMessage.Logger;
using TransactionalBox.Internals.Inbox.Storage;
using TransactionalBox.Internals.Inbox.Storage.ContractsToImplement;
using TransactionalBox.Internals.Inbox.Assemblies.MessageTypes;
using TransactionalBox.Internals.InternalPackages.EventHooks;

namespace TransactionalBox.Internals.Inbox.Hooks.Handlers.ProcessMessage
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

        private readonly IProcessMessageLogger _logger;

        public ProcessMessage(
            IServiceProvider serviceProvider,
            ICompiledInboxHandlers compiledInboxHandlers,
            IInboxStorage inboxStorage,
            IInboxDeserializer deserializer,
            IInboxMessageTypes inboxMessageTypes,
            ISystemClock systemClock,
            IEventHookPublisher eventHookPublisher,
            IProcessMessageLogger logger)
        {
            _serviceProvider = serviceProvider;
            _compiledInboxHandlers = compiledInboxHandlers;
            _inboxStorage = inboxStorage;
            _deserializer = deserializer;
            _inboxMessageTypes = inboxMessageTypes;
            _systemClock = systemClock;
            _eventHookPublisher = eventHookPublisher;
            _logger = logger;
        }

        public async Task HandleAsync(IHookExecutionContext context, CancellationToken cancellationToken)
        {
            long iteration = 1;
            var nowUtc = _systemClock.UtcNow;

            //Semaphore (process all with max limit)

            InboxMessageStorage? inboxMessage;

            do
            {
                inboxMessage = await _inboxStorage.GetMessage(context.Id, context.Name, _systemClock.TimeProvider, TimeSpan.FromSeconds(3)).ConfigureAwait(false);

                if (inboxMessage is null)
                {
                    await _eventHookPublisher.PublishAsync<ProcessedMessageFromInbox>().ConfigureAwait(false);
                    return;
                }

                var messageTypeName = inboxMessage.Topic.Split('.')[1]; //TODO

                _inboxMessageTypes.DictionaryMessageTypes.TryGetValue(messageTypeName, out var type);

                // (Error case) TODO what when type does not exist

                Type handlerType = typeof(IInboxHandler<>).MakeGenericType(type);

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

                _logger.Processed(context.Name, context.Id, inboxMessage.Id);

                if (iteration % 501 == 500) //TODO
                {
                    await _eventHookPublisher.PublishAsync<ProcessedMessageFromInbox>().ConfigureAwait(false);
                }

                iteration++;
            }
            while (inboxMessage is not null);

            await _eventHookPublisher.PublishAsync<ProcessedMessageFromInbox>().ConfigureAwait(false);
        }
    }
}
