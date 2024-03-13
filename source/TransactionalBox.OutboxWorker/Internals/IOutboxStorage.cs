using TransactionalBox.OutboxBase.StorageModel;

namespace TransactionalBox.OutboxWorker.Internals
{
    public interface IOutboxStorage
    {
        Task<IEnumerable<OutboxMessage>> GetMessages(int batchSize, DateTime nowUtc, DateTime lockUtc, string machineName);

        Task MarkAsProcessed(IEnumerable<OutboxMessage> messages, DateTime processedUtc);
    }
}
