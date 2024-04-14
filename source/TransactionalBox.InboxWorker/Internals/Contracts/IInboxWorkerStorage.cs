using TransactionalBox.Base.Inbox.StorageModel.Internals;

namespace TransactionalBox.InboxWorker.Internals.Contracts
{
    internal interface IInboxWorkerStorage
    {
        Task<IEnumerable<IdempotentInboxKey>> GetExistIdempotentInboxKeysBasedOn(IEnumerable<InboxMessage> messages);

        Task<AddRangeToInboxStorageResult> AddRange(IEnumerable<InboxMessage> messages, IEnumerable<IdempotentInboxKey> idempotentInboxKeys);

        Task<int> RemoveProcessedMessages(int batchSize);
    }
}
