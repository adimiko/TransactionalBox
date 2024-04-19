using TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution.ValueObjects;

namespace TransactionalBox.Base.BackgroundService.Internals.JobExecutors
{
    internal interface IJobExecutorLogger<T> where T : class
    {
        void StartedJob(JobExecutorId jobExecutorId, JobName jobName, JobId jobId);

        void EndedJob(JobId jobId);

        void UnexpectedError(Exception exception, long attempt, TimeSpan delay);

        void ReturnedToNormal();

    }
}
