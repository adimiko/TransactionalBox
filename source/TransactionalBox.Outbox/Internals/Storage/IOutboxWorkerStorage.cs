using TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution.ValueObjects;

namespace TransactionalBox.Outbox.Internals.Storage
{
    internal interface IOutboxWorkerStorage
    {
        Task<int> MarkMessages(JobId jobId, JobName jobName, int batchSize, TimeProvider timeProvider, TimeSpan lockTimeout);

        Task<IEnumerable<OutboxMessageStorage>> GetMarkedMessages(JobId jobId);

        Task MarkAsProcessed(JobId jobId, DateTime processedUtc);

        Task<int> RemoveProcessedMessages(int batchSize);
    }
}
