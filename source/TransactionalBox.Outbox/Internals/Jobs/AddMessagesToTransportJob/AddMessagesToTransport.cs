using System.Text;
using TransactionalBox.Base.BackgroundService.Internals;
using TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution;
using TransactionalBox.Base.BackgroundService.Internals.JobExecutors;
using TransactionalBox.Internals;
using TransactionalBox.Outbox.Internals.Compression;
using TransactionalBox.Outbox.Internals.Jobs.AddMessagesToTransportJob.TransportMessageFactories;
using TransactionalBox.Outbox.Internals.Loggers;
using TransactionalBox.Outbox.Internals.Storage;
using TransactionalBox.Outbox.Internals.Transport;

namespace TransactionalBox.Outbox.Internals.Jobs.AddMessagesToTransportJob
{
    internal sealed class AddMessagesToTransport : Job
    {
        private readonly ISystemClock _systemClock;

        private readonly IOutboxWorkerLogger<AddMessagesToTransport> _logger;

        private readonly IAddMessagesToTransportJobSettings _settings;

        private readonly IOutboxWorkerStorage _outboxStorage;

        private readonly IOutboxWorkerTransport _transport;

        private readonly IJobExecutionContext _jobExecutionContext;

        private readonly ICompressionAlgorithm _compressionAlgorithm;

        private readonly TransportMessageFactory _transportMessageFactory;

        public AddMessagesToTransport(
            ISystemClock systemClock,
            IOutboxWorkerLogger<AddMessagesToTransport> logger,
            IAddMessagesToTransportJobSettings settings,
            IOutboxWorkerStorage outboxStorage,
            IOutboxWorkerTransport transport,
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

            var batchSize = _settings.BatchSize;

            if (_jobExecutionContext.ProcessingState == ProcessingState.Error)
            {
                batchSize = 1; //TODO settings
            }

            var numberOfMessages = await _outboxStorage.MarkMessages(_jobExecutionContext.JobId, _jobExecutionContext.JobName, batchSize, _systemClock.TimeProvider, _settings.LockTimeout);

            if (numberOfMessages == 0) // IsBatchEmpty
            {
                await Task.Delay(_settings.DelayWhenBatchIsEmpty, _systemClock.TimeProvider, stoppingToken);
                return;
            }

            var messages = await _outboxStorage.GetMarkedMessages(_jobExecutionContext.JobId);

            var transportMessages = await _transportMessageFactory.Create(messages);

            foreach (var transportMessage in transportMessages)
            {
                var transportResult = await _transport.Add(transportMessage.Topic, transportMessage.Payload);

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
