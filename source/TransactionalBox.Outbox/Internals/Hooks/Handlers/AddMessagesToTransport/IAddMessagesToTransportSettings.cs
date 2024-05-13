namespace TransactionalBox.Outbox.Internals.Hooks.Handlers.AddMessagesToTransport
{
    internal interface IAddMessagesToTransportSettings
    {
        int MaxBatchSize { get; }

        TimeSpan LockTimeout { get; }
    }
}
