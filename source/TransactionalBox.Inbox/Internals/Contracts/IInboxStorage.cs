using TransactionalBox.BackgroundServiceBase.Internals.ValueObjects;
using TransactionalBox.InboxBase.StorageModel.Internals;

namespace TransactionalBox.Inbox.Internals.Contracts
{
    internal interface IInboxStorage
    {
        Task<InboxMessage?> GetMessage(JobId jobId, JobName jobName, DateTime nowUtc, TimeSpan lockTimeout);
    }
}
