using TransactionalBox.Inbox.Internals.Storage;

namespace TransactionalBox.Inbox.Storage.InMemory
{
    public interface IInboxStorageReadOnly
    {
        IReadOnlyCollection<InboxMessageStorage> InboxMessages { get; }

        IReadOnlyCollection<IdempotentInboxKey> IdempotentInboxKeys { get; }
    }
}
