namespace TransactionalBox.Internals.Inbox.InboxMessageDefinitions
{
    internal sealed class DefaultInboxMessageDefinition : IInboxMessageDefinition
    {
        public string? PublishedBy { get; } = null;
    }
}
