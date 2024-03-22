using TransactionalBox.OutboxBase.StorageModel.Internals;

namespace TransactionalBox.Outbox.Internals
{
    internal interface IOutboxStorage
    {
        Task Add(OutboxMessage message);
    }
}
