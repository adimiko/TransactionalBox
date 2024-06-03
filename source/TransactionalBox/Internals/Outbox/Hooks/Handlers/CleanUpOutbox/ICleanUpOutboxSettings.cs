namespace TransactionalBox.Internals.Outbox.Hooks.Handlers.CleanUpOutbox
{
    internal interface ICleanUpOutboxSettings
    {
        int MaxBatchSize { get; }
    }
}
