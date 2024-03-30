using System.Text;
using TransactionalBox.BackgroundServiceBase.Internals;
using TransactionalBox.BackgroundServiceBase.Internals.Context;
using TransactionalBox.Internals;
using TransactionalBox.OutboxWorker.Compression;
using TransactionalBox.OutboxWorker.Internals.Contracts;
using TransactionalBox.OutboxWorker.Internals.Loggers;

namespace TransactionalBox.OutboxWorker.Internals.Jobs
{
    internal sealed class AddMessagesToTransport : Job
    {
        private readonly ISystemClock _systemClock;

        private readonly IOutboxWorkerLogger<AddMessagesToTransport> _logger;

        private readonly IAddMessagesToTransportJobSettings _settings;

        private readonly IOutboxStorage _outboxStorage;

        private readonly ITransport _transport;

        private readonly IJobExecutionContext _jobExecutionContext;

        private readonly ICompressionAlgorithm _compressionAlgorithm;

        private readonly TransportMessageFactory _transportMessageFactory;

        public AddMessagesToTransport(
            ISystemClock systemClock,
            IOutboxWorkerLogger<AddMessagesToTransport> logger,
            IAddMessagesToTransportJobSettings settings,
            IOutboxStorage outboxStorage,
            ITransport transport,
            IJobExecutionContext jobExecutionContext,
            ICompressionAlgorithm compressionAlgorithm,
            TransportMessageFactory transportMessageFactory)
        {
            _systemClock = systemClock;
            _logger = logger;
            _settings = settings;
            _outboxStorage = outboxStorage;
            _transport = transport;
            _jobExecutionContext = jobExecutionContext;
            _compressionAlgorithm = compressionAlgorithm;
            _transportMessageFactory = transportMessageFactory;
        }

        protected override async Task Execute(CancellationToken stoppingToken)
        {
            var nowUtc = _systemClock.UtcNow;

            var numberOfMessages = await _outboxStorage.MarkMessages(_jobExecutionContext.JobId, _jobExecutionContext.JobName, _settings.BatchSize, nowUtc, _settings.LockTimeout);

            if (numberOfMessages == 0) // IsBatchEmpty
            {
                await Task.Delay(_settings.DelayWhenBatchIsEmpty, _systemClock.TimeProvider, stoppingToken);
                return;
            }

            var messages = await _outboxStorage.GetMarkedMessages(_jobExecutionContext.JobId);

            var transportMessages = _transportMessageFactory.Create(messages);

            foreach ( var message in transportMessages ) 
            {
                var payload = Encoding.UTF8.GetBytes(message.Payload);

                var compressedPayload = await _compressionAlgorithm.Compress(payload);

                var transportResult = await _transport.Add(message.Topic, compressedPayload);

                if (transportResult == TransportResult.Failure)
                {
                    //TODO log
                    //TODO Circular Breaker ?
                    _logger.FailedToAddMessagesToTransport(); //TODO
                    return;
                }
            }

            await _outboxStorage.MarkAsProcessed(_jobExecutionContext.JobId, _systemClock.UtcNow);

            if (numberOfMessages < _settings.BatchSize) // IsBatchNotFull
            {
                //TODO BackgroundJob delay based on result ?
                await Task.Delay(_settings.DelayWhenBatchIsNotFull, _systemClock.TimeProvider, stoppingToken);
            }
        }
    }
}
