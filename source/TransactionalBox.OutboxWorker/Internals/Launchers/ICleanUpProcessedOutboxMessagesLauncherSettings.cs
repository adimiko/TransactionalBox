namespace TransactionalBox.OutboxWorker.Internals.Launchers
{
    internal interface ICleanUpProcessedOutboxMessagesLauncherSettings
    {
        int NumberOfInstances { get; }
    }
}
