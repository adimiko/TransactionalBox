using TransactionalBox.BackgroundServiceBase.Internals.ValueObjects;
using TransactionalBox.OutboxBase.StorageModel;

namespace TransactionalBox.OutboxWorker.Internals
{
    public interface IOutboxStorage
    {
        Task<int> MarkMessages(JobId jobId, JobName jobName, int batchSize, DateTime nowUtc, DateTime lockUtc);

        Task<IEnumerable<OutboxMessage>> GetMessages(JobId jobId);

        Task MarkAsProcessed(JobId jobId, DateTime processedUtc);
    }
}
