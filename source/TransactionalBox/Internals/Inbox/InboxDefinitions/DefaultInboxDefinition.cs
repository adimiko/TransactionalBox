namespace TransactionalBox.Internals.Inbox.InboxDefinitions
{
    internal sealed class DefaultInboxDefinition : IInboxDefinition
    {
        public string? PublishedBy { get; } = null;
    }
}
