namespace TransactionalBox.Inbox.Internals.Hooks
{
    internal interface ICleanUpProcessedInboxMessagesJobSettings
    {
        int BatchSize { get; }
    }
}
