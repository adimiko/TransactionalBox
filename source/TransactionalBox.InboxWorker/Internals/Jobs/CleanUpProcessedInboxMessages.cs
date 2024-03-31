using TransactionalBox.BackgroundServiceBase.Internals;
using TransactionalBox.InboxWorker.Internals.Contracts;
using TransactionalBox.Internals;

namespace TransactionalBox.InboxWorker.Internals.Jobs
{
    internal sealed class CleanUpProcessedInboxMessages : Job
    {
        private readonly IInboxStorage _inboxStorage;

        private readonly ISystemClock _systemClock;

        private readonly ICleanUpProcessedInboxMessagesJobSettings _settings;

        public CleanUpProcessedInboxMessages(
            IInboxStorage inboxStorage,
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
