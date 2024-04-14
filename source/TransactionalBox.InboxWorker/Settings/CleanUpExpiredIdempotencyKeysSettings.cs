using TransactionalBox.InboxWorker.Internals.Jobs;
using TransactionalBox.InboxWorker.Internals.Launchers;

namespace TransactionalBox.InboxWorker.Settings
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
