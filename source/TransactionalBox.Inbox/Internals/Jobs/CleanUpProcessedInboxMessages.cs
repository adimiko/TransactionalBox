using TransactionalBox.Base.BackgroundService.Internals;
using TransactionalBox.InboxWorker.Internals.Contracts;
using TransactionalBox.Internals;

namespace TransactionalBox.Inbox.Internals.Jobs
{
    internal sealed class CleanUpProcessedInboxMessages : Job
    {
        private readonly IInboxWorkerStorage _inboxStorage;

        private readonly ISystemClock _systemClock;

        private readonly ICleanUpProcessedInboxMessagesJobSettings _settings;

        public CleanUpProcessedInboxMessages(
            IInboxWorkerStorage inboxStorage,
            ISystemClock systemClock,
            ICleanUpProcessedInboxMessagesJobSettings settings)
        {
            _inboxStorage = inboxStorage;
            _systemClock = systemClock;
            _settings = settings;
        }

        protected override async Task Execute(CancellationToken stoppingToken)
        {
            var numberOfRemovedMessages = await _inboxStorage.RemoveProcessedMessages(_settings.BatchSize);

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
