namespace TransactionalBox.Outbox.Internals.Hooks.CleanUpProcessedOutboxMessages
{
    internal interface ICleanUpProcessedOutboxMessagesHookSettings
    {
        int BatchSize { get; }
    }
}
