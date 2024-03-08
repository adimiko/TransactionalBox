using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace TransactionalBox.Inbox.Internals
{
    internal sealed class InboxProcessor : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IInboxMessageTypes _inboxMessageTypes;

        public InboxProcessor(
            IServiceProvider serviceProvider,
            IInboxMessageTypes inboxMessageTypes) 
        {
            _serviceProvider = serviceProvider;
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

                    //TODO #27
                    var message = JsonSerializer.Deserialize(inboxMessage.Payload, type) as IInboxMessage;

                    //TODO #39 (Performance) when program start below code can be compiled to lambda expresion
                    await (Task) handlerType
                        .GetMethod("Handle")?
                        .Invoke(handler, new object[] { message, stoppingToken });
                }
            }
        }
    }
}
