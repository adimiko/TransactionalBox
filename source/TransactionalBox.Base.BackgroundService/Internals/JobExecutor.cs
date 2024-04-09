using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Base.BackgroundService.Internals.Context;
using TransactionalBox.Base.BackgroundService.Internals.Loggers;
using TransactionalBox.Base.BackgroundService.Internals.ValueObjects;

namespace TransactionalBox.Base.BackgroundService.Internals
{
    internal sealed class JobExecutor
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IJobExecutorLogger<JobExecutor> _logger;

        public JobExecutor(
            IServiceProvider serviceProvider,
            IJobExecutorLogger<JobExecutor> logger) 
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        internal async Task Execute(Type jobType, string jobId, CancellationToken stoppingToken)
        {
            //TODO prepare
            //TODO log settings & enviroment (ProcessorCount etc.)
            //TODO error

            //_logger.Information("Settings: {0}", _settings);

            var jobExecutorId = new JobExecutorId(Guid.NewGuid());

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var jobExecutionContextConstructor = scope.ServiceProvider.GetRequiredService<IJobExecutionContextConstructor>();

                        var localJobId = new JobId(jobId);
                        var jobName = new JobName(jobType.Name);

                        jobExecutionContextConstructor.JobId = localJobId;
                        jobExecutionContextConstructor.JobExecutorId = jobExecutorId;
                        jobExecutionContextConstructor.JobName = jobName;

                        Job job = scope.ServiceProvider.GetRequiredService(jobType) as Job;

                        _logger.StartedJob(jobExecutorId, jobName, localJobId);

                        await job.Execute(stoppingToken);

                        _logger.EndedJob(localJobId);
                    }
                }
                catch (Exception ex)
                {
                    _logger.UnexpectedError(ex);
                }
            }
        }
    }
}
