﻿using TransactionalBox.InboxBase.StorageModel.Internals;

namespace TransactionalBox.InboxWorker.Internals.Contracts
{
    internal interface IInboxStorage
    {
        Task<IEnumerable<IdempotentInboxKey>> GetExistIdempotentInboxKeysBasedOn(IEnumerable<InboxMessage> messages);

        Task<AddRangeToInboxStorageResult> AddRange(IEnumerable<InboxMessage> messages, DateTime nowUtc);
    }
}
