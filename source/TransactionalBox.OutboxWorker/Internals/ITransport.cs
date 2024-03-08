using TransactionalBox.OutboxBase.StorageModel;

namespace TransactionalBox.OutboxWorker.Internals
{
    public interface ITransport
    {
        Task Add(OutboxMessage message);
    }
}
