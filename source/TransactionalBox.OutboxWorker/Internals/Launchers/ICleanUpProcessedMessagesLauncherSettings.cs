namespace TransactionalBox.OutboxWorker.Internals.Launchers
{
    internal interface ICleanUpProcessedMessagesLauncherSettings
    {
        int NumberOfInstances { get; }
    }
}
