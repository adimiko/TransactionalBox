using TransactionalBox.BackgroundServiceBase.Internals.ValueObjects;
using TransactionalBox.OutboxBase.StorageModel;

namespace TransactionalBox.OutboxWorker.Internals
{
    public interface IOutboxStorage
    {
        Task<IEnumerable<OutboxMessage>> GetMessages(JobId jobId, int batchSize, DateTime nowUtc, DateTime lockUtc);

        Task MarkAsProcessed(JobId jobId, DateTime processedUtc);
    }
}
