using TransactionalBox.InboxBase.StorageModel;

namespace TransactionalBox.InboxWorker.Internals
{
    public sealed record DuplicatedInboxKey
    {
        public Guid Id { get; }

        public DuplicatedInboxKey(IdempotentInboxKey inboxMessage)
        {
            Id = inboxMessage.Id;
        }
    }
}
