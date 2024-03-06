using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TransactionalBox.OutboxWorker.Internals
{
    internal sealed class OutboxProcessor : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public OutboxProcessor(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //TODO prepare
            //TODO log settings & enviroment (ProcessorCount etc.)
            //TODO error

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var outbox = scope.ServiceProvider.GetRequiredService<IOutboxStorage>();
                    var transport = scope.ServiceProvider.GetRequiredService<ITransport>();

                    var messages = await outbox.GetMessages();

                    foreach (var message in messages) 
                    {
                        await transport.Add(message.Payload, message.Topic);
                    }

                    await outbox.MarkAsProcessed(messages);

                    await Task.Delay(500);
                }
            }

            //TODO End
        }
    }
}
