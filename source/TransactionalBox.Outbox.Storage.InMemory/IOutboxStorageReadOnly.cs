using TransactionalBox.Outbox.Internals.Storage;

namespace TransactionalBox.Outbox.Storage.InMemory
{
    public interface IOutboxStorageReadOnly
    {
        IReadOnlyCollection<OutboxMessage> OutboxMessages { get; }
    }
}
