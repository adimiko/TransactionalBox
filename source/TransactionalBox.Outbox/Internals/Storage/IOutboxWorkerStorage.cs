namespace TransactionalBox.Outbox.Internals.Storage
{
    internal interface IOutboxWorkerStorage
    {
        Task<int> MarkMessages(Guid hookId, string hookName, int batchSize, TimeProvider timeProvider, TimeSpan lockTimeout);

        Task<IEnumerable<OutboxMessageStorage>> GetMarkedMessages(Guid hookId);

        Task MarkAsProcessed(Guid hookId, DateTime processedUtc);

        Task<int> RemoveProcessedMessages(int batchSize);
    }
}
