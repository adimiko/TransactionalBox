using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TransactionalBox.BackgroundServiceBase.Internals;
using TransactionalBox.Internals;

namespace TransactionalBox.OutboxWorker.Internals
{
    internal sealed class OutboxProcessor : BackgroundProcess
    {
        private readonly ISystemClock _systemClock;

        private readonly IEnvironmentContext _environmentContext;

        private readonly ITransactionalBoxLogger _logger;

        private readonly IServiceProvider _serviceProvider;

        private readonly IOutboxProcessorSettings _settings;

        public OutboxProcessor(
            ISystemClock systemClock,
            IEnvironmentContext environmentContext,
            ITransactionalBoxLogger logger,
            IServiceProvider serviceProvider,
            IOutboxProcessorSettings settings) 
        {
            _systemClock = systemClock;
            _environmentContext = environmentContext;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _settings = settings;
        }

        protected override async Task Execute(string processId, CancellationToken stoppingToken)
        {
            //TODO prepare
            //TODO log settings & enviroment (ProcessorCount etc.)
            //TODO error

            //_logger.Information("Settings: {0}", _settings);

            //TODO machineName and porcess ?
            var machineNameWithProcessId = _environmentContext.MachineName + '-' + processId;

            _logger.Error($"!!!!!!Start {machineNameWithProcessId}");

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var outboxStorage = scope.ServiceProvider.GetRequiredService<IOutboxStorage>();
                    var transport = scope.ServiceProvider.GetRequiredService<ITransport>();

                    var nowUtc = _systemClock.UtcNow;
                    var lockUtc = nowUtc + _settings.LockTimeout;

                    //TODO mutex
                    var messages = await outboxStorage.GetMessages(_settings.BatchSize, nowUtc, lockUtc, machineNameWithProcessId);

                    var numberOfMessages = messages.Count();

                    var isBatchEmpty = numberOfMessages == 0;

                    if (isBatchEmpty)
                    {
                        await Task.Delay(_settings.DelayWhenBatchIsEmpty, _systemClock.TimeProvider, stoppingToken);
                        continue;
                    }

                    foreach (var message in messages) 
                    {
                        await transport.Add(message);
                    }

                    await outboxStorage.MarkAsProcessed(messages, _systemClock.UtcNow);

                    var isBatchNotFull = numberOfMessages < _settings.BatchSize;

                    if (isBatchNotFull)
                    {
                        await Task.Delay(_settings.DelayWhenBatchIsNotFull, _systemClock.TimeProvider, stoppingToken);
                    }

                    _logger.Information("TEST LOG");
                }
            }

            //TODO End
        }
    }
}
