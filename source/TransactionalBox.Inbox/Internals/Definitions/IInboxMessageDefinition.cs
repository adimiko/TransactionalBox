namespace TransactionalBox.Inbox.Internals.Definitions
{
    internal interface IInboxMessageDefinition
    {
        string? PublishedBy { get; }
    }
}