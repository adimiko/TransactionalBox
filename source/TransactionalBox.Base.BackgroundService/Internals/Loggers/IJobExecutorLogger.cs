﻿using TransactionalBox.Base.BackgroundService.Internals.ValueObjects;

namespace TransactionalBox.Base.BackgroundService.Internals.Loggers
{
    internal interface IJobExecutorLogger<T> where T : class
    {
        void StartedJob(JobExecutorId jobExecutorId, JobName jobName, JobId jobId);

        void EndedJob(JobId jobId);

        void UnexpectedError(Exception exception);
    }
}