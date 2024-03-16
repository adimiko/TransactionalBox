using TransactionalBox.OutboxBase.StorageModel;

namespace TransactionalBox.OutboxWorker.Internals
{
    public interface IOutboxStorage
    {
        Task<IEnumerable<OutboxMessage>> GetMessages(string jobId, int batchSize, DateTime nowUtc, DateTime lockUtc);

        Task MarkAsProcessed(string jobId, DateTime processedUtc);
    }
}
