using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.BackgroundServiceBase.Internals
{
    internal sealed class ParallelExecutor : IParallelExecutor
    {
        private readonly IServiceProvider _serviceProvider;

        public ParallelExecutor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<Task> Run<T>(int numberOfInstances, CancellationToken stoppingToken)
            where T : BackgroundProcess
        {
            //TODO valid run the same processes
            List<Task> _tasks = new List<Task>();

            for (var i = 1; i <= numberOfInstances; i++)
            {
                var processName = typeof(T).Name;
                var processId = processName + i;

                var task = Task.Run(() => _serviceProvider.GetRequiredService<T>().Execute(processId, stoppingToken));

                _tasks.Add(task);
            }

            return _tasks;
        }

    }
}
