namespace TransactionalBox.Inbox.Internals.Hooks.Handlers.CleanUpInbox
{
    internal interface ICleanUpInboxSettings
    {
        int BatchSize { get; }
    }
}
