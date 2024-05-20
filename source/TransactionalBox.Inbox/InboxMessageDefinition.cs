using TransactionalBox.Inbox.Internals.InboxMessageDefinitions;

namespace TransactionalBox
{
    public abstract class InboxMessageDefinition<TInboxMessage> : IInboxMessageDefinition
        where TInboxMessage : InboxMessage, new()
    {
        protected internal string? PublishedBy { get; protected set; } = null;

        string? IInboxMessageDefinition.PublishedBy => PublishedBy;

        //TODO RetryConfiguration (poisoned message)
    }
}
