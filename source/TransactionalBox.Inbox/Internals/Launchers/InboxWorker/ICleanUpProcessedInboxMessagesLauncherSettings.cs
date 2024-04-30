namespace TransactionalBox.Inbox.Internals.Launchers.InboxWorker
{
    internal interface ICleanUpProcessedInboxMessagesLauncherSettings
    {
        int NumberOfInstances { get; }
    }
}
