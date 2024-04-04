using TransactionalBox.BackgroundServiceBase.Internals.ValueObjects;
using TransactionalBox.OutboxBase.StorageModel.Internals;

namespace TransactionalBox.OutboxWorker.Internals.Contracts
{
    internal interface IOutboxWorkerStorage
    {
        Task<int> MarkMessages(JobId jobId, JobName jobName, int batchSize, DateTime nowUtc, TimeSpan lockTimeout);

        Task<IEnumerable<OutboxMessage>> GetMarkedMessages(JobId jobId);

        Task MarkAsProcessed(JobId jobId, DateTime processedUtc);

        Task<int> RemoveProcessedMessages(int batchSize);
    }
}
