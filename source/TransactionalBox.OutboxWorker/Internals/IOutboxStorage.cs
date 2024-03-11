using TransactionalBox.OutboxBase.StorageModel;

namespace TransactionalBox.OutboxWorker.Internals
{
    public interface IOutboxStorage
    {
        Task<IEnumerable<OutboxMessage>> GetMessages();

        Task MarkAsProcessed(IEnumerable<OutboxMessage> messages, DateTime processedUtc);
    }
}
