using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.BackgroundServiceBase.Internals;
using TransactionalBox.Inbox.Deserialization;

namespace TransactionalBox.Inbox.Internals.Jobs
{
    internal sealed class ProcessMessageFromInboxStorage : Job
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IInboxStorage _inboxStorage;

        private readonly IInboxDeserializer _deserializer;

        private readonly IInboxMessageTypes _inboxMessageTypes;

        public ProcessMessageFromInboxStorage(
            IServiceProvider serviceProvider,
            IInboxStorage inboxStorage,
            IInboxDeserializer deserializer,
            IInboxMessageTypes inboxMessageTypes)
        {
            _serviceProvider = serviceProvider;
            _inboxStorage = inboxStorage;
            _deserializer = deserializer;
            _inboxMessageTypes = inboxMessageTypes;
        }

        protected override async Task Execute(CancellationToken stoppingToken)
        {
            var inboxMessage = await _inboxStorage.GetMessage();

            if (inboxMessage is null)
            {
                await Task.Delay(500);

                return;
            }

            var messageTypeName = inboxMessage.Topic.Split('-')[1];

            _inboxMessageTypes.Types.TryGetValue(messageTypeName, out var type);

            // (Error case) TODO what when type does not exist

            Type handlerType = typeof(IInboxMessageHandler<>).MakeGenericType(type);

            var handler = _serviceProvider.GetRequiredService(handlerType);

            var message = _deserializer.Deserialize(inboxMessage.Data, type);

            //TODO #39 (Performance) when program start below code can be compiled to lambda expresion
            await (Task)handlerType
                .GetMethod("Handle")?
                .Invoke(handler, new object[] { message, stoppingToken });
        }
    }
}
