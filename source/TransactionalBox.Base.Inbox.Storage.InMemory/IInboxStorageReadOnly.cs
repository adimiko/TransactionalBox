using TransactionalBox.InboxBase.StorageModel.Internals;

namespace TransactionalBox.Base.Inbox.Storage.InMemory
{
    public interface IInboxStorageReadOnly
    {
        IReadOnlyCollection<InboxMessage> InboxMessages { get; }

        IReadOnlyCollection<IdempotentInboxKey> IdempotentInboxKeys { get; }
    }
}
