using TransactionalBox.Internals.Inbox.InboxMessageDefinitions;

namespace TransactionalBox
{
    /// <summary>
    /// Define the inbox message.
    /// </summary>
    public abstract class InboxMessageDefinition<TInboxMessage> : IInboxMessageDefinition
        where TInboxMessage : InboxMessage, new()
    {
        protected internal string? PublishedBy { get; protected set; } = null;

        string? IInboxMessageDefinition.PublishedBy => PublishedBy;

        //TODO RetryConfiguration (poisoned message)
    }
}
