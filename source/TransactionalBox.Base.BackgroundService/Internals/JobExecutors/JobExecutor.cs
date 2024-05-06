using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution.ValueObjects;
using TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution;
using TransactionalBox.Base.BackgroundService.Internals.JobExecutors.Loggers;
using TransactionalBox.Base.BackgroundService.Internals.Jobs;

namespace TransactionalBox.Base.BackgroundService.Internals.JobExecutors
{
    internal sealed class JobExecutor
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly TimeProvider _timeProvider;

        public JobExecutor(
            IServiceProvider serviceProvider,
            TimeProvider timeProvider)
        {
            _serviceProvider = serviceProvider;
            _timeProvider = timeProvider;
        }

        public async Task Execute(Type jobType, string jobId, CancellationToken stoppingToken)
        {
            //_logger.Information("Settings: {0}", _settings);

            var jobExecutorId = new JobExecutorId(Guid.NewGuid());

            var processingState = ProcessingState.Normal;
            long attempt = 0;

            var logger = _serviceProvider.GetRequiredService(typeof(IJobExecutorLogger<>).MakeGenericType(jobType)) as IJobExecutorLogger;


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

                    var job = scope.ServiceProvider.GetRequiredService(jobType) as GeneralJob;

                    logger.StartedJob(jobExecutorId, localJobId);

                    await job.Execute(stoppingToken);

                    logger.EndedJob(localJobId);
                }

                if (processingState == ProcessingState.Error)
                {
                    processingState = ProcessingState.Normal;
                    attempt = 0;

                    logger.ReturnedToNormal();
                }
            }
            catch (Exception ex)
            {
                processingState = ProcessingState.Error;

                attempt++;

                TimeSpan delay = TimeSpan.FromSeconds(10); //TODO settings

                if (attempt <= 10)
                {
                    delay = TimeSpan.FromSeconds(attempt);
                }

                logger.UnexpectedError(ex, attempt, delay);

                //TODO await Task.Delay(delay, _timeProvider, stoppingToken);
            }
        }
    }
}
