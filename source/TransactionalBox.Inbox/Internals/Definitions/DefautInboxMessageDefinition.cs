namespace TransactionalBox.Inbox.Internals.Definitions
{
    internal sealed class DefautInboxMessageDefinition : IInboxMessageDefinition
    {
        public string? PublishedBy { get; } = null;
    }
}
