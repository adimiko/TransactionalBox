using TransactionalBox.OutboxBase.StorageModel;

namespace TransactionalBox.OutboxWorker.Internals
{
    public interface ITransport
    {
        Task AddRange(IEnumerable<OutboxMessage> messages);
    }
}
