using TransactionalBox.Inbox.Internals.Jobs;
using TransactionalBox.Inbox.Internals.Launchers;

namespace TransactionalBox.Inbox.Settings
{
    public sealed class CleanUpProcessedInboxMessagesSettings : ICleanUpProcessedInboxMessagesJobSettings
    {
        public int BatchSize { get; set; } = 10000;

        public bool IsEnabled { get; set; } = true;

        internal CleanUpProcessedInboxMessagesSettings() { }
    }
}