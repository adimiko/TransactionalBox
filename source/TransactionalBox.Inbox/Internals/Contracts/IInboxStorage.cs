using TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution.ValueObjects;
using TransactionalBox.Inbox.Internals.Storage;

namespace TransactionalBox.Inbox.Internals.Contracts
{
    internal interface IInboxStorage
    {
        Task<InboxMessageStorage?> GetMessage(JobId jobId, JobName jobName, TimeProvider timeProvider, TimeSpan lockTimeout);
    }
}
