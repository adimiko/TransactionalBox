using TransactionalBox.Base.BackgroundService.Internals;
using TransactionalBox.Internals;
using TransactionalBox.OutboxWorker.Internals.Contracts;

namespace TransactionalBox.OutboxWorker.Internals.Jobs
{
    internal sealed class CleanUpProcessedOutboxMessages : Job
    {
        private readonly IOutboxWorkerStorage _outboxStorage;

        private readonly ICleanUpProcessedOutboxMessagesJobSettings _settings;

        private readonly ISystemClock _systemClock;

        public CleanUpProcessedOutboxMessages(
            IOutboxWorkerStorage outboxStorage,
            ICleanUpProcessedOutboxMessagesJobSettings settings,
            ISystemClock systemClock) 
        {
            _outboxStorage = outboxStorage;
            _settings = settings;
            _systemClock = systemClock;
        }

        protected override async Task Execute(CancellationToken stoppingToken)
        {
            var numberOfRemovedMessages = await _outboxStorage.RemoveProcessedMessages(_settings.BatchSize);

            if (numberOfRemovedMessages == 0) // IsBatchEmpty
            {
                await Task.Delay(_settings.DelayWhenBatchIsEmpty, _systemClock.TimeProvider, stoppingToken);
                return;
            }

            if (numberOfRemovedMessages < _settings.BatchSize) // IsBatchNotFull
            {
                await Task.Delay(_settings.DelayWhenBatchIsNotFull, _systemClock.TimeProvider, stoppingToken);
            }
        }
    }
}
