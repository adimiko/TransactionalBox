namespace TransactionalBox.Outbox.Internals.Hooks.Handlers.AddMessagesToTransport
{
    internal interface IAddMessagesToTransportSettings
    {
        int BatchSize { get; }

        TimeSpan LockTimeout { get; }
    }
}
