namespace TransactionalBox.InboxWorker.Internals.Settings
{
    internal interface IInboxWorkerLauncherSettings
    {
        public int NumberOfAddMessagesToInboxStorageJobExecutors { get; }
    }
}
