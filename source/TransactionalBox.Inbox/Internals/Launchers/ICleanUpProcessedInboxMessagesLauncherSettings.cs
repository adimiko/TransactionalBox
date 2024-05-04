namespace TransactionalBox.Inbox.Internals.Launchers
{
    internal interface ICleanUpProcessedInboxMessagesLauncherSettings
    {
        int NumberOfInstances { get; }
    }
}
