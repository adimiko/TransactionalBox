using TransactionalBox.Outbox.Internals.Hooks.CleanUpProcessedOutboxMessages;

namespace TransactionalBox.Outbox.Settings
{
    public sealed class CleanUpProcessedOutboxMessagesSettings : ICleanUpProcessedOutboxMessagesHookSettings
    {
        public int BatchSize { get; set; } = 10000;

        public bool IsEnabled { get; set; } = true;

        internal CleanUpProcessedOutboxMessagesSettings() { }
    }
}
