using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TransactionalBox.Internals;

namespace TransactionalBox.OutboxWorker.Internals
{
    internal sealed class OutboxProcessor : BackgroundService
    {
        private readonly ISystemClock _systemClock;

        private readonly ITransactionalBoxLogger _logger;

        private readonly IServiceProvider _serviceProvider;

        public OutboxProcessor(
            ISystemClock systemClock,
            ITransactionalBoxLogger logger,
            IServiceProvider serviceProvider) 
        {
            _systemClock = systemClock;
            _logger = logger;
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
                        await transport.Add(message);
                    }

                    await outbox.MarkAsProcessed(messages, _systemClock.UtcNow);

                    _logger.Information("TEST LOG");

                    await Task.Delay(TimeSpan.FromMicroseconds(500), _systemClock.TimeProvider, stoppingToken);
                }
            }

            //TODO End
        }
    }
}
