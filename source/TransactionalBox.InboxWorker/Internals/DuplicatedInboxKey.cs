using TransactionalBox.Base.Inbox.StorageModel.Internals;

namespace TransactionalBox.InboxWorker.Internals
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
