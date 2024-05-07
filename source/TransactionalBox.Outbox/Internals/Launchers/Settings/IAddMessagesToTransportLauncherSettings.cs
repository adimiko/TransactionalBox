namespace TransactionalBox.Outbox.Internals.Launchers.Settings
{
    internal interface IAddMessagesToTransportLauncherSettings
    {
        int NumberOfInstances { get; }
    }
}
