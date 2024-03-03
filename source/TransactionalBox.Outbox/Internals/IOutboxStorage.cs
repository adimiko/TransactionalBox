using TransactionalBox.OutboxBase.StorageModel;

namespace TransactionalBox.Outbox.Internals
{
    public interface IOutboxStorage
    {
        Task Add(OutboxMessage message);
    }
}
