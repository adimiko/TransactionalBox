using TransactionalBox.Inbox.Internals.BackgroundProcesses.CleanUpIdempotencyKeys;

namespace TransactionalBox.Inbox.Settings
{
    public sealed class CleanUpIdempotencyKeysSettings : ICleanUpIdempotencyKeysSettings
    {
        public int BatchSize { get; set; } = 10000;

        public TimeSpan Period { get; set; } = TimeSpan.FromHours(1); 

        internal CleanUpIdempotencyKeysSettings() { }
    }
}
