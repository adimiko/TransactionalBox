namespace TransactionalBox.Internals.Inbox.InboxMessageDefinitions
{
    internal interface IInboxMessageDefinition
    {
        string? PublishedBy { get; }
    }
}