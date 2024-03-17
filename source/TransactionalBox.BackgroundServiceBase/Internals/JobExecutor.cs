﻿using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.BackgroundServiceBase.Internals.Context;
using TransactionalBox.BackgroundServiceBase.Internals.ValueObjects;
using TransactionalBox.Internals;

namespace TransactionalBox.BackgroundServiceBase.Internals
{
    internal sealed class JobExecutor
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly ITransactionalBoxLogger _logger;

        public JobExecutor(
            IServiceProvider serviceProvider,
            ITransactionalBoxLogger logger) 
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

            var jobExecutorId = Guid.NewGuid(); //TODO


            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var jobExecutionContextConstructor = scope.ServiceProvider.GetRequiredService<IJobExecutionContextConstructor>();

                        jobExecutionContextConstructor.JobId = new JobId(jobId);
                        jobExecutionContextConstructor.JobExecutorId = jobExecutorId.ToString();//TODO
                        jobExecutionContextConstructor.JobName = new JobName(jobType.Name);

                        Job job = scope.ServiceProvider.GetRequiredService(jobType) as Job;

                        await job.Execute(stoppingToken);
                    }
                }
            }
            catch (Exception ex) 
            {
                _logger.Error(ex, "Error");
            }
        }

    }
}
