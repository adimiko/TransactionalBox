namespace TransactionalBox.Inbox.Internals.InboxMessageDefinitions
{
    internal sealed class DefaultInboxMessageDefinition : IInboxMessageDefinition
    {
        public string? PublishedBy { get; } = null;
    }
}
