using TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution.ValueObjects;

namespace TransactionalBox.Base.BackgroundService.Internals.JobExecutors.Loggers
{


    internal interface IJobExecutorLogger
    {
        void StartedJob(JobExecutorId jobExecutorId, JobId jobId);

        void EndedJob(JobId jobId);

        void UnexpectedError(Exception exception, long attempt, TimeSpan delay);

        void ReturnedToNormal();

    }

    internal interface IJobExecutorLogger<T> : IJobExecutorLogger
        where T : class;
}
