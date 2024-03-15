using TransactionalBox.BackgroundServiceBase.Internals;
using TransactionalBox.Internals;

namespace TransactionalBox.OutboxWorker.Internals
{
    internal sealed class OutboxProcessor : Job
    {
        private readonly ISystemClock _systemClock;

        private readonly IEnvironmentContext _environmentContext;

        private readonly ITransactionalBoxLogger _logger;

        private readonly IOutboxProcessorSettings _settings;

        private readonly IOutboxStorage _outboxStorage;

        private readonly ITransport _transport;

        public OutboxProcessor(
            ISystemClock systemClock,
            IEnvironmentContext environmentContext,
            ITransactionalBoxLogger logger,
            IOutboxProcessorSettings settings,
            IOutboxStorage outboxStorage,
            ITransport transport) 
        {
            _systemClock = systemClock;
            _environmentContext = environmentContext;
            _logger = logger;
            _settings = settings;
            _outboxStorage = outboxStorage;
            _transport = transport;
        }

        protected override async Task Execute(string processId, CancellationToken stoppingToken)
        {
            var jobExecutionId = _environmentContext.MachineName + Guid.NewGuid();

            _logger.Information("Start job with id: {0}", jobExecutionId);

            var nowUtc = _systemClock.UtcNow;
            var lockUtc = nowUtc + _settings.LockTimeout;

            var messages = await _outboxStorage.GetMessages(jobExecutionId, _settings.BatchSize, nowUtc, lockUtc);

            var numberOfMessages = messages.Count();

            if (numberOfMessages == 0) // IsBatchEmpty
            {
                await Task.Delay(_settings.DelayWhenBatchIsEmpty, _systemClock.TimeProvider, stoppingToken);
                return;
            }

            foreach (var message in messages)
            {
                await _transport.Add(message);
            }

            //TODO (ADR) get messages added to transport (with result success save to db) OR (All success or failed) 

            await _outboxStorage.MarkAsProcessed(messages, _systemClock.UtcNow);

            if (numberOfMessages < _settings.BatchSize) // IsBatchNotFull
            {
                await Task.Delay(_settings.DelayWhenBatchIsNotFull, _systemClock.TimeProvider, stoppingToken);
            }

            _logger.Information("End job with id: {0}", jobExecutionId);
        }
    }
}
