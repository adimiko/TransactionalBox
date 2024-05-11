using TransactionalBox.Inbox.Internals.BackgroundProcesses;

namespace TransactionalBox.Inbox.Settings
{
    public sealed class CleanUpExpiredIdempotencyKeysSettings : ICleanUpExpiredIdempotencyKeysJobSettings
    {
        public int BatchSize { get; set; } = 10000;

        public TimeSpan Period { get; set; } = TimeSpan.FromHours(1); 

        internal CleanUpExpiredIdempotencyKeysSettings() { }
    }
}
