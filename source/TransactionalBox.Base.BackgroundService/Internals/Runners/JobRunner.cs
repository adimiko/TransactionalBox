using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Base.BackgroundService.Internals.Contexts.Environment;
using TransactionalBox.Base.BackgroundService.Internals.JobExecutors;

namespace TransactionalBox.Base.BackgroundService.Internals.Runners
{
    internal sealed class JobRunner
    {
        private readonly IServiceProvider _serviceProvider;


        private readonly IEnvironmentContext _environmentContext;

        private readonly JobIdGenerator _jobIdGenerator;

        public JobRunner(
            IServiceProvider serviceProvider,
            IEnvironmentContext environmentContext,
            JobIdGenerator jobIdGenerator)
        {
            _serviceProvider = serviceProvider;
            _environmentContext = environmentContext;
            _jobIdGenerator = jobIdGenerator;
        }

        public Task Run(Type jobType, CancellationToken stoppingToken)
        {
            Task task;

            using (var scope = _serviceProvider.CreateScope())
            {
                var jobId = _jobIdGenerator.GetId(_environmentContext.MachineName, jobType.Name, 0);

                task = _serviceProvider.GetRequiredService<JobExecutor>().Execute(jobType, jobId, stoppingToken);
            }

            return task;
        }
    }
}
