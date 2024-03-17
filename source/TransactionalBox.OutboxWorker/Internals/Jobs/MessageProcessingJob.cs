using TransactionalBox.BackgroundServiceBase.Internals;
using TransactionalBox.BackgroundServiceBase.Internals.Context;
using TransactionalBox.Internals;

namespace TransactionalBox.OutboxWorker.Internals.Jobs
{
    internal sealed class MessageProcessingJob : Job
    {
        private readonly ISystemClock _systemClock;

        private readonly ITransactionalBoxLogger _logger;

        private readonly IOutboxProcessorSettings _settings;

        private readonly IOutboxStorage _outboxStorage;

        private readonly ITransport _transport;

        private readonly IJobExecutionContext _jobExecutionContext;

        public MessageProcessingJob(
            ISystemClock systemClock,
            ITransactionalBoxLogger logger,
            IOutboxProcessorSettings settings,
            IOutboxStorage outboxStorage,
            ITransport transport,
            IJobExecutionContext jobExecutionContext)
        {
            _systemClock = systemClock;
            _logger = logger;
            _settings = settings;
            _outboxStorage = outboxStorage;
            _transport = transport;
            _jobExecutionContext = jobExecutionContext;
        }

        protected override async Task Execute(CancellationToken stoppingToken)
        {
            _logger.Information("Start job with id: {0}", _jobExecutionContext.JobId.ToString());

            var nowUtc = _systemClock.UtcNow;
            var lockUtc = nowUtc + _settings.LockTimeout;

            var numberOfMessages = await _outboxStorage.MarkMessages(_jobExecutionContext.JobId, _jobExecutionContext.JobName, _settings.BatchSize, nowUtc, lockUtc);

            if (numberOfMessages == 0) // IsBatchEmpty
            {
                await Task.Delay(_settings.DelayWhenBatchIsEmpty, _systemClock.TimeProvider, stoppingToken);
                return;
            }

            var messages = await _outboxStorage.GetMessages(_jobExecutionContext.JobId);

            foreach (var message in messages)
            {
                await _transport.Add(message);
            }

            //TODO (ADR) get messages added to transport (with result success save to db) OR (All success or failed) 

            await _outboxStorage.MarkAsProcessed(_jobExecutionContext.JobId, _systemClock.UtcNow);

            if (numberOfMessages < _settings.BatchSize) // IsBatchNotFull
            {
                await Task.Delay(_settings.DelayWhenBatchIsNotFull, _systemClock.TimeProvider, stoppingToken);
            }

            _logger.Information("End job with id: {0}", _jobExecutionContext.JobId);
        }
    }
}
