namespace TransactionalBox.Outbox.Internals.Launchers
{
    internal interface ICleanUpProcessedOutboxMessagesLauncherSettings
    {
        int NumberOfInstances { get; }
    }
}
