﻿using TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution.ValueObjects;
using TransactionalBox.Base.Inbox.StorageModel.Internals;

namespace TransactionalBox.Inbox.Internals.Contracts
{
    internal interface IInboxStorage
    {
        Task<InboxMessage?> GetMessage(JobId jobId, JobName jobName, TimeProvider timeProvider, TimeSpan lockTimeout);
    }
}
