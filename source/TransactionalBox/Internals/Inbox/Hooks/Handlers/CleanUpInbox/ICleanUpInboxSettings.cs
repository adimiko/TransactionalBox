namespace TransactionalBox.Internals.Inbox.Hooks.Handlers.CleanUpInbox
{
    internal interface ICleanUpInboxSettings
    {
        int MaxBatchSize { get; }
    }
}
