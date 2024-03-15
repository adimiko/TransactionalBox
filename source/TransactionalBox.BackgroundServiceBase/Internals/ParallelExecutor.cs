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

        public IEnumerable<Task> Run<T>(int numberOfInstances, CancellationToken stoppingToken)
            where T : Job
        {
            //TODO valid run the same processes
            List<Task> _tasks = new List<Task>();

            for (var i = 1; i <= numberOfInstances; i++)
            {
                var jobId = _jobIdGenerator.GetId(_environmentContext.MachineName, typeof(T).Name, i);

                var task = Task.Run(() => _serviceProvider.GetRequiredService<JobExecutor>().Execute<T>(jobId, stoppingToken));

                _tasks.Add(task);
            }

            return _tasks;
        }

    }
}
