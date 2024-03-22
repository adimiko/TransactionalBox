using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TransactionalBox.Internals;

namespace TransactionalBox.BackgroundServiceBase.Internals
{
    public abstract class Launcher : BackgroundService
    {
        private readonly List<JobLaunchSettings> _jobLaunchSettings = new();

        private readonly IServiceProvider _serviceProvider;

        protected Launcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected void Launch<T>(int numberOfInstances) where T : Job
        {
            _jobLaunchSettings.Add(new JobLaunchSettings(typeof(T), numberOfInstances));
        }

        protected sealed override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var parallelExecutor = _serviceProvider.GetRequiredService<IParallelExecutor>();
            var logger = _serviceProvider.GetRequiredService<ITransactionalBoxLogger>();
            //TODO log settings

            try
            {
                var startedTasks = new List<Task>();

                foreach(var settings in _jobLaunchSettings)
                {
                    var tasks = await parallelExecutor.Run(settings.JobType, settings.NumberOfInstances, stoppingToken);

                    startedTasks.AddRange(tasks);
                }

                await Task.WhenAll(startedTasks);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                logger.Error(ex, "Error");
            }
        }
    }
}
