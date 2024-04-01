using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using TransactionalBox.BackgroundServiceBase.Internals;
using TransactionalBox.BackgroundServiceBase.Internals.Context;
using TransactionalBox.Inbox.Contexts;
using TransactionalBox.Inbox.Deserialization;
using TransactionalBox.Inbox.Internals.Contracts;
using TransactionalBox.Internals;

namespace TransactionalBox.Inbox.Internals.Jobs
{
    internal sealed class ProcessMessageFromInboxStorage : Job
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IInboxStorage _inboxStorage;

        private readonly IInboxDeserializer _deserializer;

        private readonly IInboxMessageTypes _inboxMessageTypes;

        private readonly IJobExecutionContext _jobExecutionContext;

        private readonly ISystemClock _systemClock;

        public ProcessMessageFromInboxStorage(
            IServiceProvider serviceProvider,
            IInboxStorage inboxStorage,
            IInboxDeserializer deserializer,
            IInboxMessageTypes inboxMessageTypes,
            IJobExecutionContext jobExecutionContext,
            ISystemClock systemClock)
        {
            _serviceProvider = serviceProvider;
            _inboxStorage = inboxStorage;
            _deserializer = deserializer;
            _inboxMessageTypes = inboxMessageTypes;
            _jobExecutionContext = jobExecutionContext;
            _systemClock = systemClock;
        }

        protected override async Task Execute(CancellationToken stoppingToken)
        {
            var jobId = _jobExecutionContext.JobId;
            var jobName = _jobExecutionContext.JobName;
            var nowUtc = _systemClock.UtcNow;

            var inboxMessage = await _inboxStorage.GetMessage(jobId, jobName, nowUtc, TimeSpan.FromMinutes(1));

            if (inboxMessage is null)
            {
                await Task.Delay(500);

                return;
            }

            var messageTypeName = inboxMessage.Topic.Split('.')[1]; //TODO

            _inboxMessageTypes.Types.TryGetValue(messageTypeName, out var type);

            // (Error case) TODO what when type does not exist

            Type handlerType = typeof(IInboxMessageHandler<>).MakeGenericType(type);

            var handler = _serviceProvider.GetRequiredService(handlerType);

            string metadataJson;
            string messageJson;

            using (var jsonDocument = JsonDocument.Parse(inboxMessage.Data))
            {
                var jsonRoot = jsonDocument.RootElement;

                metadataJson = jsonRoot.GetProperty("Metadata").ToString();
                messageJson = jsonRoot.GetProperty("Message").ToString();
            }

            var metadata = _deserializer.DeserializeMetadata(metadataJson);
            var message = _deserializer.DeserializeMessage(messageJson, type);

            IExecutionContext executionContext = new ExecutionContext(metadata, stoppingToken);

            //TODO #39 (Performance) when program start below code can be compiled to lambda expresion
            await (Task)handlerType
                .GetMethod("Handle")?
                .Invoke(handler, new object[] { message, executionContext });
        }
    }
}
