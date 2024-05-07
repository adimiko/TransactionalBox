using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Base.BackgroundService.Internals.Launchers.Loggers;

namespace TransactionalBox.Base.BackgroundService.Internals.Launchers
{
    public abstract class Launcher : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly List<JobLaunchSettings> _jobLaunchSettings;

        private readonly IServiceProvider _serviceProvider;

        protected Launcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _jobLaunchSettings = new List<JobLaunchSettings>();
        }

        protected void Launch<T>(int numberOfInstances) where T : Job
        {
            _jobLaunchSettings.Add(new JobLaunchSettings(typeof(T), numberOfInstances));
        }

        protected sealed override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var parallelExecutor = _serviceProvider.GetRequiredService<IParallelExecutor>();
            var logger = _serviceProvider.GetRequiredService<ILauncherLogger<Launcher>>();
            //TODO log settings
            //TODO evenDistribtuion

            try
            {
                var startedTasks = new List<Task>();

                foreach (var settings in _jobLaunchSettings)
                {
                    var tasks = await parallelExecutor.Run(settings.JobType, settings.NumberOfInstances, stoppingToken);

                    startedTasks.AddRange(tasks);
                }

                await Task.WhenAll(startedTasks);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                logger.UnexpectedError(ex);
            }
        }
    }
}
