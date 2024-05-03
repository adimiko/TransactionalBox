using TransactionalBox.Base.BackgroundService.Internals;
using TransactionalBox.Inbox.Internals.Storage;
using TransactionalBox.Internals;

namespace TransactionalBox.Inbox.Internals.Jobs
{
    internal sealed class CleanUpExpiredIdempotencyKeys : Job
    {
        private readonly IInboxWorkerStorage _inboxStorage;

        private readonly ISystemClock _systemClock;

        private readonly ICleanUpExpiredIdempotencyKeysJobSettings _settings;

        public CleanUpExpiredIdempotencyKeys(IInboxWorkerStorage inboxStorage, ISystemClock systemClock, ICleanUpExpiredIdempotencyKeysJobSettings settings)
        {
            _inboxStorage = inboxStorage;
            _systemClock = systemClock;
            _settings = settings;
        }

        protected override async Task Execute(CancellationToken stoppingToken)
        {
            var numberOfRemovedMessages = await _inboxStorage.RemoveExpiredIdempotencyKeys(_settings.BatchSize, _systemClock.UtcNow);

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
