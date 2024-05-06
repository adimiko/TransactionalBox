﻿namespace TransactionalBox.Inbox.Internals.Storage
{
    //TODO Storage interface per job
    internal interface IInboxWorkerStorage
    {
        Task<IEnumerable<IdempotentInboxKey>> GetExistIdempotentInboxKeysBasedOn(IEnumerable<InboxMessageStorage> messages);

        Task<AddRangeToInboxStorageResult> AddRange(IEnumerable<InboxMessageStorage> messages, IEnumerable<IdempotentInboxKey> idempotentInboxKeys);

        Task<int> RemoveProcessedMessages(int batchSize);

        Task<int> RemoveExpiredIdempotencyKeys(int batchSize, DateTime nowUtc);
    }
}