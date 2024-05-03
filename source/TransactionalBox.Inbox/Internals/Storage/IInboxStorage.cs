using TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution.ValueObjects;

namespace TransactionalBox.Inbox.Internals.Storage
{
    internal interface IInboxStorage
    {
        Task<InboxMessageStorage?> GetMessage(JobId jobId, JobName jobName, TimeProvider timeProvider, TimeSpan lockTimeout);
    }
}
