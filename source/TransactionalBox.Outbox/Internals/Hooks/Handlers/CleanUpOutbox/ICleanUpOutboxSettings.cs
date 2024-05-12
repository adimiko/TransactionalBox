namespace TransactionalBox.Outbox.Internals.Hooks.Handlers.CleanUpOutbox
{
    internal interface ICleanUpOutboxSettings
    {
        int BatchSize { get; }
    }
}
