using TransactionalBox.BackgroundServiceBase.Internals.ValueObjects;

namespace TransactionalBox.BackgroundServiceBase.Internals.Loggers
{
    internal interface IJobExecutorLogger<T> where T : class
    {
        void StartedJob(JobExecutorId jobExecutorId, JobName jobName, JobId jobId);

        void EndedJob(JobId jobId);
    }
}
