using Microsoft.Extensions.Logging;
using TransactionalBox.Base.BackgroundService.Internals.ValueObjects;

namespace TransactionalBox.Base.BackgroundService.Internals.Loggers
{
    internal sealed partial class JobExecutorLogger<T> : IJobExecutorLogger<T>
        where T : class
    {
        private readonly ILogger<T> _logger;

        public JobExecutorLogger(ILogger<T> logger) 
        {
            _logger = logger;
        }
        //TODO source generation
        private static Action<ILogger, string, Exception?> _endedJob = LoggerMessage.Define<string>(LogLevel.Trace, 0, "Ended job '{jobId}'");

        public void EndedJob(JobId jobId) => _endedJob(_logger, jobId.ToString(), null);

        private static Action<ILogger, Guid, string, string, Exception?> _startedJob = LoggerMessage.Define<Guid, string, string>(LogLevel.Trace, 1, "Job executor '{jobExecutorId}' started '{jobName}' job '{jobId}'");

        public void StartedJob(JobExecutorId jobExecutorId, JobName jobName, JobId jobId) => _startedJob(_logger, jobExecutorId.Value, jobName.Value, jobId.Value, null);
        

        [LoggerMessage(0, LogLevel.Error, "Unexpected exception (Attempt: {attempt} Delay: {delay})")]
        public partial void UnexpectedError(Exception exception, long attempt, TimeSpan delay);

        [LoggerMessage(1, LogLevel.Information, "Returned to normal")]
        public partial void ReturnedToNormal();
    }
}
