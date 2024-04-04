using TransactionalBox.OutboxBase.StorageModel.Internals;

namespace TransactionalBox.Base.Outbox.Storage.InMemory
{
    public interface IOutboxStorageReadOnly
    {
        IReadOnlyCollection<OutboxMessage> OutboxMessages { get; }
    }
}
