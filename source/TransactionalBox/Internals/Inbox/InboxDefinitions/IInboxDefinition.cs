namespace TransactionalBox.Internals.Inbox.InboxDefinitions
{
    internal interface IInboxDefinition
    {
        string? PublishedBy { get; }
    }
}