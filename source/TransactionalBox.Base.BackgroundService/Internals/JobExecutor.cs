using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using TransactionalBox.Base.BackgroundService.Internals.Context;
using TransactionalBox.Base.BackgroundService.Internals.Loggers;
using TransactionalBox.Base.BackgroundService.Internals.Context.ValueObjects;

namespace TransactionalBox.Base.BackgroundService.Internals
{
    internal sealed class JobExecutor
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IJobExecutorLogger<JobExecutor> _logger;

        private readonly TimeProvider _timeProvider;

        public JobExecutor(
            IServiceProvider serviceProvider,
            IJobExecutorLogger<JobExecutor> logger,
            TimeProvider timeProvider) 
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _timeProvider = timeProvider;
        }

        internal async Task Execute(Type jobType, string jobId, CancellationToken stoppingToken)
        {
            //TODO prepare
            //TODO log settings & enviroment (ProcessorCount etc.)
            //TODO error

            //_logger.Information("Settings: {0}", _settings);

            var jobExecutorId = new JobExecutorId(Guid.NewGuid());

            var processingState = ProcessingState.Normal;
            long attempt = 0;

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
                        jobExecutionContextConstructor.ProcessingState = processingState;

                        Job job = scope.ServiceProvider.GetRequiredService(jobType) as Job;

                        _logger.StartedJob(jobExecutorId, jobName, localJobId);

                        await job.Execute(stoppingToken);

                        _logger.EndedJob(localJobId);
                    }

                    if (processingState == ProcessingState.Error)
                    {
                        processingState = ProcessingState.Normal;
                        attempt = 0;

                        _logger.ReturnedToNormal(); //TODO jaki which job logging
                    }
                }
                catch (Exception ex)
                {
                    processingState = ProcessingState.Error;

                    attempt++;

                    TimeSpan delay = TimeSpan.FromSeconds(10); //TODO settings

                    if (attempt <= 10) //TODO settings
                    {
                        delay = TimeSpan.FromSeconds(attempt);
                    }

                    _logger.UnexpectedError(ex, attempt, delay);

                    await Task.Delay(delay, _timeProvider, stoppingToken);
                }
            }
        }
    }
}
