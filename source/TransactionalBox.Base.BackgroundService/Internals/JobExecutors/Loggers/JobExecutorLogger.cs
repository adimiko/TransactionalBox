using Microsoft.Extensions.Logging;
using TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution.ValueObjects;
using TransactionalBox.Base.BackgroundService.Internals.JobExecutors.Loggers;

namespace TransactionalBox.Base.BackgroundService.Internals.Loggers
{
    internal sealed partial class JobExecutorLogger<T> : IJobExecutorLogger<T>
        where T : class
    {
        private readonly ILogger<T> _logger;

        public JobExecutorLogger(ILogger<T> logger) => _logger = logger;

        [LoggerMessage(0, LogLevel.Trace, "Ended job '{jobId}'")]
        public partial void EndedJob(JobId jobId);


        [LoggerMessage(0, LogLevel.Trace, "Job executor '{jobExecutorId}' started job '{jobId}'")]
        public partial void StartedJob(JobExecutorId jobExecutorId, JobId jobId);
        

        [LoggerMessage(0, LogLevel.Error, "Unexpected exception (Attempt: {attempt} Delay: {delay})", SkipEnabledCheck = true)]
        public partial void UnexpectedError(Exception exception, long attempt, TimeSpan delay);

        [LoggerMessage(0, LogLevel.Information, "Returned to normal", SkipEnabledCheck = true)]
        public partial void ReturnedToNormal();
    }
}
