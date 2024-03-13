using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TransactionalBox.Internals;

namespace TransactionalBox.OutboxWorker.Internals
{
    internal sealed class OutboxProcessor : BackgroundService
    {
        private readonly ISystemClock _systemClock;

        private readonly IHostMachine _hostMachine;

        private readonly ITransactionalBoxLogger _logger;

        private readonly IServiceProvider _serviceProvider;

        public OutboxProcessor(
            ISystemClock systemClock,
            IHostMachine hostMachine,
            ITransactionalBoxLogger logger,
            IServiceProvider serviceProvider) 
        {
            _systemClock = systemClock;
            _hostMachine = hostMachine;
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
                    var outboxStorage = scope.ServiceProvider.GetRequiredService<IOutboxStorage>();
                    var transport = scope.ServiceProvider.GetRequiredService<ITransport>();
                    
                    var machineName = _hostMachine.Name;
                    var lockTimeout = TimeSpan.FromSeconds(15); // TODO settings
                    var packageSize = 1000;  // TODO settings

                    var nowUtc = _systemClock.UtcNow;
                    var lockUtc = nowUtc + lockTimeout;

                    var messages = await outboxStorage.GetMessages(packageSize, nowUtc, lockUtc, machineName);

                    foreach (var message in messages) 
                    {
                        await transport.Add(message);
                    }

                    await outboxStorage.MarkAsProcessed(messages, _systemClock.UtcNow);

                    _logger.Information("TEST LOG");

                    await Task.Delay(TimeSpan.FromMicroseconds(500), _systemClock.TimeProvider, stoppingToken);
                }
            }

            //TODO End
        }
    }
}
