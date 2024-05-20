namespace TransactionalBox.Inbox.Internals.InboxMessageDefinitions
{
    internal interface IInboxMessageDefinition
    {
        string? PublishedBy { get; }
    }
}