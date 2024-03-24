using TransactionalBox.BackgroundServiceBase.Internals;
using TransactionalBox.BackgroundServiceBase.Internals.Context;
using TransactionalBox.Internals;
using TransactionalBox.OutboxWorker.Internals.Contracts;
using TransactionalBox.OutboxWorker.Internals.Loggers;

namespace TransactionalBox.OutboxWorker.Internals.Jobs
{
    internal sealed class AddMessagesToTransport : Job
    {
        private readonly ISystemClock _systemClock;

        private readonly IOutboxWorkerLogger<AddMessagesToTransport> _logger;

        private readonly IOutboxProcessorSettings _settings;

        private readonly IOutboxStorage _outboxStorage;

        private readonly ITransport _transport;

        private readonly IJobExecutionContext _jobExecutionContext;

        private readonly TransportMessageFactory _transportMessageFactory;

        public AddMessagesToTransport(
            ISystemClock systemClock,
            IOutboxWorkerLogger<AddMessagesToTransport> logger,
            IOutboxProcessorSettings settings,
            IOutboxStorage outboxStorage,
            ITransport transport,
            IJobExecutionContext jobExecutionContext,
            TransportMessageFactory transportMessageFactory)
        {
            _systemClock = systemClock;
            _logger = logger;
            _settings = settings;
            _outboxStorage = outboxStorage;
            _transport = transport;
            _jobExecutionContext = jobExecutionContext;
            _transportMessageFactory = transportMessageFactory;
        }

        protected override async Task Execute(CancellationToken stoppingToken)
        {
            //_logger.Information("Start job with id: {0}", _jobExecutionContext.JobId.ToString());

            var nowUtc = _systemClock.UtcNow;
            var lockUtc = nowUtc + _settings.LockTimeout;

            var numberOfMessages = await _outboxStorage.MarkMessages(_jobExecutionContext.JobId, _jobExecutionContext.JobName, _settings.BatchSize, nowUtc, lockUtc);

            if (numberOfMessages == 0) // IsBatchEmpty
            {
                await Task.Delay(_settings.DelayWhenBatchIsEmpty, _systemClock.TimeProvider, stoppingToken);
                return;
            }

            var messages = await _outboxStorage.GetMarkedMessages(_jobExecutionContext.JobId);

            var transportMessages = _transportMessageFactory.Create(messages);

            var transportResult = await _transport.Add(transportMessages);

            if (transportResult == TransportResult.Failure)
            {
                //TODO log
                //TODO Circular Breaker ?
                _logger.FailedToAddMessagesToTransport(); //TODO
                return;
            }

            await _outboxStorage.MarkAsProcessed(_jobExecutionContext.JobId, _systemClock.UtcNow);

            if (numberOfMessages < _settings.BatchSize) // IsBatchNotFull
            {
                //TODO BackgroundJob delay based on result ?
                await Task.Delay(_settings.DelayWhenBatchIsNotFull, _systemClock.TimeProvider, stoppingToken);
            }

            //_logger.Information("End job with id: {0}", _jobExecutionContext.JobId);
        }
    }
}
