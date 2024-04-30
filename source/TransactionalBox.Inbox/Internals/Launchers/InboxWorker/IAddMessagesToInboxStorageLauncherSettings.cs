namespace TransactionalBox.Inbox.Internals.Launchers.InboxWorker
{
    internal interface IAddMessagesToInboxStorageLauncherSettings
    {
        public int NumberOfInstances { get; }
    }
}
