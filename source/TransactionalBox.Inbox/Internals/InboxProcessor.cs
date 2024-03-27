using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using TransactionalBox.Inbox.Deserialization;

namespace TransactionalBox.Inbox.Internals
{
    internal sealed class InboxProcessor : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IInboxDeserializer _deserializer;

        private readonly IInboxMessageTypes _inboxMessageTypes;

        public InboxProcessor(
            IServiceProvider serviceProvider,
            IInboxDeserializer deserializer,
            IInboxMessageTypes inboxMessageTypes) 
        {
            _serviceProvider = serviceProvider;
            _deserializer = deserializer;
            _inboxMessageTypes = inboxMessageTypes;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var inboxStorage = scope.ServiceProvider.GetRequiredService<IInboxStorage>();

                    var inboxMessage = await inboxStorage.GetMessage();

                    if (inboxMessage is null) 
                    {
                        await Task.Delay(500);

                        continue;
                    }

                    var messageTypeName = inboxMessage.Topic.Split('-')[1];

                    _inboxMessageTypes.Types.TryGetValue(messageTypeName, out var type);

                    // (Error case) TODO what when type does not exist

                    Type handlerType = typeof(IInboxMessageHandler<>).MakeGenericType(type);

                    var handler = scope.ServiceProvider.GetRequiredService(handlerType);

                    var message = _deserializer.Deserialize(inboxMessage.Data, type);

                    //TODO #39 (Performance) when program start below code can be compiled to lambda expresion
                    await (Task) handlerType
                        .GetMethod("Handle")?
                        .Invoke(handler, new object[] { message, stoppingToken });
                }
            }
        }
    }
}
