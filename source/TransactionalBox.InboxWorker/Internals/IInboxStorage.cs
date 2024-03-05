﻿using TransactionalBox.InboxBase.StorageModel;

namespace TransactionalBox.InboxWorker.Internals
{
    public interface IInboxStorage
    {
        Task Add(InboxMessageStorageModel message);
    }
}
