using TransactionalBox.Outbox.Internals.Hooks.Handlers.CleanUpOutbox;

namespace TransactionalBox.Outbox.Settings
{
    public sealed class CleanUpOutboxSettings : ICleanUpOutboxSettings
    {
        public int BatchSize { get; set; } = 10000;

        public bool IsEnabled { get; set; } = true;

        internal CleanUpOutboxSettings() { }
    }
}
