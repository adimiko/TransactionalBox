using TransactionalBox.Inbox.Internals.Definitions;

namespace TransactionalBox.Inbox
{
    public abstract class InboxMessageDefinition<TInboxMessage> : IInboxMessageDefinition
        where TInboxMessage : InboxMessage, new()
    {
        protected internal string? PublishedBy { get; protected set; } = null;

        string? IInboxMessageDefinition.PublishedBy => PublishedBy;

        //TODO RetryConfiguration (poisoned message)
    }
}
