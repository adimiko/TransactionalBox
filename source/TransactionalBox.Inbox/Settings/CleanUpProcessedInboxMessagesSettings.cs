using TransactionalBox.Inbox.Internals.Jobs;
using TransactionalBox.Inbox.Internals.Launchers;

namespace TransactionalBox.Inbox.Settings
{
    public sealed class CleanUpProcessedInboxMessagesSettings : ICleanUpProcessedInboxMessagesJobSettings, ICleanUpProcessedInboxMessagesLauncherSettings
    {
        public int BatchSize { get; set; } = 10000;

        public int NumberOfInstances { get; set; } = 1;

        public TimeSpan DelayWhenBatchIsEmpty { get; set; } = TimeSpan.FromSeconds(5);

        public TimeSpan DelayWhenBatchIsNotFull { get; set; } = TimeSpan.FromSeconds(1);

        internal CleanUpProcessedInboxMessagesSettings() { }
    }
}