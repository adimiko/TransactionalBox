namespace TransactionalBox.Internals.Outbox.Hooks.Handlers.AddMessagesToTransport
{
    internal interface IAddMessagesToTransportSettings
    {
        int MaxBatchSize { get; }

        TimeSpan LockTimeout { get; }
    }
}
