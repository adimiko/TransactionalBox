using TransactionalBox.InboxWorker.Internals.Settings;

namespace TransactionalBox.InboxWorker.Settings
{
    public sealed class InboxWorkerSettings : IInboxWorkerLauncherSettings
    {
        public int NumberOfAddMessagesToInboxStorageJobExecutors { get; set; } = 1;

        internal InboxWorkerSettings() { }
    }
}
