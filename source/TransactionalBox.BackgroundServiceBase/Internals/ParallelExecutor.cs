using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Internals;

namespace TransactionalBox.BackgroundServiceBase.Internals
{
    internal sealed class ParallelExecutor : IParallelExecutor
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IEnvironmentContext _environmentContext;

        private readonly JobIdGenerator _jobIdGenerator;

        public ParallelExecutor(
            IServiceProvider serviceProvider,
            IEnvironmentContext environmentContext,
            JobIdGenerator jobIdGenerator)
        {
            _serviceProvider = serviceProvider;
            _environmentContext = environmentContext;
            _jobIdGenerator = jobIdGenerator;
        }

        public IEnumerable<Task> Run(Type jobType, int numberOfInstances, CancellationToken stoppingToken)
        {
            //TODO valid run the same processes
            List<Task> _tasks = new List<Task>();

            for (var i = 1; i <= numberOfInstances; i++)
            {
                var jobId = _jobIdGenerator.GetId(_environmentContext.MachineName, jobType.Name, i);

                var task = Task.Run(() => _serviceProvider.GetRequiredService<JobExecutor>().Execute(jobType, jobId, stoppingToken));

                _tasks.Add(task);
            }

            return _tasks;
        }
    }
}
