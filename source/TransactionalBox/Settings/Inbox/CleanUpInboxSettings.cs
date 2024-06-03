using TransactionalBox.Internals.Inbox.Hooks.Handlers.CleanUpInbox;

namespace TransactionalBox.Settings.Inbox
{
    public sealed class CleanUpInboxSettings : ICleanUpInboxSettings
    {
        public int MaxBatchSize { get; set; } = 10000;

        public bool IsEnabled { get; set; } = true;

        internal CleanUpInboxSettings() { }
    }
}