namespace TransactionalBox.Outbox.Internals.Launchers
{
    internal interface IAddMessagesToTransportLauncherSettings
    {
        int NumberOfInstances { get; }
    }
}
