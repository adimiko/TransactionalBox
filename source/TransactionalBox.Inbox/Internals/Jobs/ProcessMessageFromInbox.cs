using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using TransactionalBox.Base.BackgroundService.Internals;
using TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution;
using TransactionalBox.Inbox.Contexts;
using TransactionalBox.Inbox.Internals.Assemblies.CompiledHandlers;
using TransactionalBox.Inbox.Internals.Assemblies.MessageTypes;
using TransactionalBox.Inbox.Internals.Deserialization;
using TransactionalBox.Inbox.Internals.Storage;
using TransactionalBox.Internals;

namespace TransactionalBox.Inbox.Internals.Jobs
{
    internal sealed class ProcessMessageFromInbox : Job
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly ICompiledInboxHandlers _compiledInboxHandlers;

        private readonly IInboxStorage _inboxStorage;

        private readonly IInboxDeserializer _deserializer;

        private readonly IInboxMessageTypes _inboxMessageTypes;

        private readonly IJobExecutionContext _jobExecutionContext;

        private readonly ISystemClock _systemClock;

        private readonly IProcessMessageFromInboxJobSettings _settings;

        public ProcessMessageFromInbox(
            IServiceProvider serviceProvider,
            ICompiledInboxHandlers compiledInboxHandlers,
            IInboxStorage inboxStorage,
            IInboxDeserializer deserializer,
            IInboxMessageTypes inboxMessageTypes,
            IJobExecutionContext jobExecutionContext,
            ISystemClock systemClock,
            IProcessMessageFromInboxJobSettings settings)
        {
            _serviceProvider = serviceProvider;
            _compiledInboxHandlers = compiledInboxHandlers;
            _inboxStorage = inboxStorage;
            _deserializer = deserializer;
            _inboxMessageTypes = inboxMessageTypes;
            _jobExecutionContext = jobExecutionContext;
            _systemClock = systemClock;
            _settings = settings;
        }

        protected override async Task Execute(CancellationToken stoppingToken)
        {
            var jobId = _jobExecutionContext.JobId;
            var jobName = _jobExecutionContext.JobName;
            var nowUtc = _systemClock.UtcNow;

            var inboxMessage = await _inboxStorage.GetMessage(jobId, jobName, _systemClock.TimeProvider, TimeSpan.FromMinutes(1));

            if (inboxMessage is null)
            {
                await Task.Delay(_settings.DelayWhenInboxIsEmpty, _systemClock.TimeProvider);

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

            IExecutionContext executionContext = new Contexts.ExecutionContext(metadata, stoppingToken);

            var compiledHandler = _compiledInboxHandlers.GetCompiledInboxHandler(type);

            await compiledHandler(handler, message, executionContext);
        }
    }
}
