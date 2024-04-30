using TransactionalBox.Inbox.Internals.Jobs;
using TransactionalBox.Inbox.Internals.Launchers.InboxWorker;

namespace TransactionalBox.Inbox.Settings.InboxWorker
{
    public sealed class CleanUpExpiredIdempotencyKeysSettings : ICleanUpExpiredIdempotencyKeysLauncherSettings, ICleanUpExpiredIdempotencyKeysJobSettings
    {
        public int BatchSize { get; set; } = 10000;

        public int NumberOfInstances { get; set; } = 1;

        public TimeSpan DelayWhenBatchIsEmpty { get; set; } = TimeSpan.FromHours(1);

        public TimeSpan DelayWhenBatchIsNotFull { get; set; } = TimeSpan.FromMinutes(1);

        internal CleanUpExpiredIdempotencyKeysSettings() { }
    }
}
