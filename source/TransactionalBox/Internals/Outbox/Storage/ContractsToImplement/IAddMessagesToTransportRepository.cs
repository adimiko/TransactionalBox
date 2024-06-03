namespace TransactionalBox.Internals.Outbox.Storage.ContractsToImplement
{
    internal interface IAddMessagesToTransportRepository
    {
        Task<int> MarkMessages(Guid hookId, string hookName, int batchSize, TimeProvider timeProvider, TimeSpan lockTimeout);

        Task<IEnumerable<OutboxMessageStorage>> GetMarkedMessages(Guid hookId);

        Task MarkAsProcessed(Guid hookId, DateTime processedUtc);
    }
}
