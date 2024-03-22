using TransactionalBox.BackgroundServiceBase.Internals.ValueObjects;
using TransactionalBox.OutboxBase.StorageModel.Internals;

namespace TransactionalBox.OutboxWorker.Internals.Contracts
{
    internal interface IOutboxStorage
    {
        Task<int> MarkMessages(JobId jobId, JobName jobName, int batchSize, DateTime nowUtc, DateTime lockUtc);

        Task<IEnumerable<OutboxMessage>> GetMarkedMessages(JobId jobId);

        Task MarkAsProcessed(JobId jobId, DateTime processedUtc);
    }
}
