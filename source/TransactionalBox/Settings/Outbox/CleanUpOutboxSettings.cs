using TransactionalBox.Internals.Outbox.Hooks.Handlers.CleanUpOutbox;

namespace TransactionalBox.Settings.Outbox
{
    public sealed class CleanUpOutboxSettings : ICleanUpOutboxSettings
    {
        public int MaxBatchSize { get; set; } = 10000;

        public bool IsEnabled { get; set; } = true;

        internal CleanUpOutboxSettings() { }
    }
}
