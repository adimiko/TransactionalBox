using TransactionalBox.Base.Outbox.StorageModel.Internals;

namespace TransactionalBox.Base.Outbox.Storage.InMemory
{
    public interface IOutboxStorageReadOnly
    {
        IReadOnlyCollection<OutboxMessage> OutboxMessages { get; }
    }
}
