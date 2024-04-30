using TransactionalBox.Inbox.Internals.Jobs;
using TransactionalBox.Inbox.Internals.Launchers.InboxWorker;

namespace TransactionalBox.Inbox.Settings.InboxWorker
{
    public sealed class AddMessagesToInboxStorageSettings : IAddMessagesToInboxStorageLauncherSettings, IAddMessagesToInboxStorageJobSettings
    {
        public TimeSpan DefaultTimeToLiveIdempotencyKey { get; set; } = TimeSpan.FromDays(7);

        public int NumberOfInstances { get; set; } = 2;

        internal AddMessagesToInboxStorageSettings() { }
    }
}
