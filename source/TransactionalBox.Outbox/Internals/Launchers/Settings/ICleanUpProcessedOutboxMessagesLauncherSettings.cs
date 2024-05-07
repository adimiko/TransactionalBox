namespace TransactionalBox.Outbox.Internals.Launchers.Settings
{
    internal interface ICleanUpProcessedOutboxMessagesLauncherSettings
    {
        int NumberOfInstances { get; }
    }
}
