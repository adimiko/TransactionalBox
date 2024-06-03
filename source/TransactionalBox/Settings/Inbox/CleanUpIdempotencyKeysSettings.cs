using TransactionalBox.Internals.Inbox.BackgroundProcesses.CleanUpIdempotencyKeys;

namespace TransactionalBox.Settings.Inbox
{
    public sealed class CleanUpIdempotencyKeysSettings : ICleanUpIdempotencyKeysSettings
    {
        public int MaxBatchSize { get; set; } = 10000;

        public TimeSpan Period { get; set; } = TimeSpan.FromHours(1);

        internal CleanUpIdempotencyKeysSettings() { }
    }
}
