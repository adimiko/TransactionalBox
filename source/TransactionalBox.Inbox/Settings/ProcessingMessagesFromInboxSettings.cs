using TransactionalBox.Inbox.Internals.Jobs;
using TransactionalBox.Inbox.Internals.Launchers;

namespace TransactionalBox.Inbox.Settings
{
    public sealed class ProcessingMessagesFromInboxSettings : IProcessingMessagesFromInboxLauncherSettings, IProcessMessageFromInboxJobSettings
    {
        public int NumberOfInstances { get; set; } = 4;

        public TimeSpan DelayWhenInboxIsEmpty { get; set; } = TimeSpan.FromMilliseconds(100);

        internal ProcessingMessagesFromInboxSettings() { }
    }
}
