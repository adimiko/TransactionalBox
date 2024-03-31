namespace TransactionalBox.InboxWorker.Internals.Launchers
{
    internal interface ICleanUpProcessedInboxMessagesLauncherSettings
    {
        int NumberOfInstances { get; }
    }
}
