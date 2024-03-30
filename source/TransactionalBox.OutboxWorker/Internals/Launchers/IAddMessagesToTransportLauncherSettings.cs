namespace TransactionalBox.OutboxWorker.Internals.Launchers
{
    internal interface IAddMessagesToTransportLauncherSettings
    {
        int NumberOfInstances { get; }
    }
}
