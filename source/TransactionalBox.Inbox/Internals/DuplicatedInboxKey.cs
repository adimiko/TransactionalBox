using TransactionalBox.Inbox.Internals.Storage;

namespace TransactionalBox.Inbox.Internals
{
    internal sealed record DuplicatedInboxKey
    {
        internal Guid Id { get; }

        internal DuplicatedInboxKey(IdempotentInboxKey inboxMessage)
        {
            Id = inboxMessage.Id;
        }
    }
}
