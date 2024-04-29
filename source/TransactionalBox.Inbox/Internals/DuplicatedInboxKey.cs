using TransactionalBox.Base.Inbox.StorageModel.Internals;

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
