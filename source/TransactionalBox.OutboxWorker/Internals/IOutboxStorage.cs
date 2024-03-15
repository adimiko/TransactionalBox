using TransactionalBox.OutboxBase.StorageModel;

namespace TransactionalBox.OutboxWorker.Internals
{
    public interface IOutboxStorage
    {
        Task<IEnumerable<OutboxMessage>> GetMessages(string jobExecutionId, int batchSize, DateTime nowUtc, DateTime lockUtc);

        Task MarkAsProcessed(IEnumerable<OutboxMessage> messages, DateTime processedUtc);
    }
}
