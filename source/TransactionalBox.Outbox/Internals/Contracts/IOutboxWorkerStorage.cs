using TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution.ValueObjects;
using TransactionalBox.Outbox.Internals.Storage;

namespace TransactionalBox.Outbox.Internals.Contracts
{
    internal interface IOutboxWorkerStorage
    {
        Task<int> MarkMessages(JobId jobId, JobName jobName, int batchSize, TimeProvider timeProvider, TimeSpan lockTimeout);

        Task<IEnumerable<OutboxMessage>> GetMarkedMessages(JobId jobId);

        Task MarkAsProcessed(JobId jobId, DateTime processedUtc);

        Task<int> RemoveProcessedMessages(int batchSize);
    }
}
