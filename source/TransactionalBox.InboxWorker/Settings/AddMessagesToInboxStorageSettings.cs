using TransactionalBox.InboxWorker.Internals.Launchers;

namespace TransactionalBox.InboxWorker.Settings
{
    public sealed class AddMessagesToInboxStorageSettings : IAddMessagesToInboxStorageLauncherSettings
    {
        public int NumberOfInstances { get; set; } = 2;

        internal AddMessagesToInboxStorageSettings() { }
    }
}
