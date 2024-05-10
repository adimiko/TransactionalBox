namespace TransactionalBox.Outbox.Internals.Hooks.AddMessagesToTransport
{
    internal interface IAddMessagesToTransportHookSettings
    {
        int BatchSize { get; }

        TimeSpan LockTimeout { get; }
    }
}
