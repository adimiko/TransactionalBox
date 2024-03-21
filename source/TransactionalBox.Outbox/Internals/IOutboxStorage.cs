using TransactionalBox.OutboxBase.StorageModel;

namespace TransactionalBox.Outbox.Internals
{
    internal interface IOutboxStorage
    {
        Task Add(OutboxMessage message);
    }
}
