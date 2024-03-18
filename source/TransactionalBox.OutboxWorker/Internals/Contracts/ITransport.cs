using TransactionalBox.OutboxBase.StorageModel;

namespace TransactionalBox.OutboxWorker.Internals.Contracts
{
    public interface ITransport
    {
        Task AddRange(IEnumerable<OutboxMessage> messages);
    }
}
