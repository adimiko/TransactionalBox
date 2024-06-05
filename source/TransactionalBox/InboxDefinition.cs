using TransactionalBox.Internals.Inbox.InboxDefinitions;

namespace TransactionalBox
{
    /// <summary>
    /// Define the inbox message.
    /// </summary>
    public abstract class InboxDefinition<TInboxMessage> : IInboxDefinition
        where TInboxMessage : InboxMessage, new()
    {
        protected internal string? PublishedBy { get; protected set; } = null;

        string? IInboxDefinition.PublishedBy => PublishedBy;

        //TODO RetryConfiguration (poisoned message)
    }
}
