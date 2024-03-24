using Microsoft.Extensions.Logging;
using TransactionalBox.BackgroundServiceBase.Internals.ValueObjects;

namespace TransactionalBox.BackgroundServiceBase.Internals.Loggers
{
    internal sealed class JobExecutorLogger<T> : IJobExecutorLogger<T>
        where T : class
    {
        private readonly ILogger<T> _logger;

        public JobExecutorLogger(ILogger<T> logger) 
        {
            _logger = logger;
        }

        private static Action<ILogger, string, Exception?> _endedJob = LoggerMessage.Define<string>(LogLevel.Trace, 0, "Ended job '{jobId}'");

        public void EndedJob(JobId jobId) => _endedJob(_logger, jobId.ToString(), null);

        private static Action<ILogger, Guid, string, string, Exception?> _startedJob = LoggerMessage.Define<Guid, string, string>(LogLevel.Trace, 1, "Job executor '{jobExecutorId}' started '{jobName}' job '{jobId}'");

        public void StartedJob(JobExecutorId jobExecutorId, JobName jobName, JobId jobId) => _startedJob(_logger, jobExecutorId.Value, jobName.Value, jobId.Value, null);
    }
}
