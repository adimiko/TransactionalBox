using TransactionalBox.InboxWorker.Internals.Jobs;
using TransactionalBox.InboxWorker.Internals.Launchers;

namespace TransactionalBox.InboxWorker.Settings
{
    public sealed class AddMessagesToInboxStorageSettings : IAddMessagesToInboxStorageLauncherSettings, IAddMessagesToInboxStorageJobSettings
    {
        public TimeSpan DefaultTimeToLiveIdempotencyKey { get; set; } = TimeSpan.FromDays(7);

        public int NumberOfInstances { get; set; } = 2;

        internal AddMessagesToInboxStorageSettings() { }
    }
}
