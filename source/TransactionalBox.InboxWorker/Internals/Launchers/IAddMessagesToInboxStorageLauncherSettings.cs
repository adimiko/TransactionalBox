namespace TransactionalBox.InboxWorker.Internals.Launchers
{
    internal interface IAddMessagesToInboxStorageLauncherSettings
    {
        public int NumberOfInstances { get; }
    }
}
