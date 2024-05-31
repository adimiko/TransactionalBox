using TransactionalBox.Inbox.Internals.Hooks.Handlers.CleanUpInbox;

namespace TransactionalBox.Inbox.Settings
{
    public sealed class CleanUpInboxSettings : ICleanUpInboxSettings
    {
        public int MaxBatchSize { get; set; } = 10000;

        public bool IsEnabled { get; set; } = true;

        internal CleanUpInboxSettings() { }
    }
}