using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Base.BackgroundService.Internals.Jobs;
using TransactionalBox.Base.BackgroundService.Internals.Runners;
using TransactionalBox.Base.BackgroundService.Internals.Throttling;

namespace TransactionalBox.Base.BackgroundService.Internals.Launchers
{
    public abstract class Launcher : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly List<JobLaunchSettings> _jobLaunchSettings;

        private readonly List<JobLaunchSettings> _longRunningJobLaunchSettings;

        private readonly IServiceProvider _serviceProvider;

        protected Launcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _jobLaunchSettings = new List<JobLaunchSettings>();
            _longRunningJobLaunchSettings = new List<JobLaunchSettings>();
        }

        protected void LaunchJob<T>(int numberOfInstances) 
            where T : Job
        {
            _jobLaunchSettings.Add(new JobLaunchSettings(typeof(T), numberOfInstances));
        }

        protected void LaunchLongRunningJob<T>(int numberOfInstances) 
            where T : LongRunningJob
        {
            _longRunningJobLaunchSettings.Add(new JobLaunchSettings(typeof(T), numberOfInstances));
        }

        protected sealed override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var parallelExecutor = _serviceProvider.GetRequiredService<ILongRunningJobRunner>();
            var logger = _serviceProvider.GetRequiredService<ILauncherLogger<Launcher>>();
            var infinityJobsIteration = _serviceProvider.GetRequiredService<IInfinityJobsIteration>();
            //TODO log settings
            //TODO evenDistribtuion

            // Long Running Jobs
            var longRunningJobs = new List<Task>();

            try
            {
                foreach (var settings in _jobLaunchSettings)
                {
                    var tasks = await parallelExecutor.Run(settings.JobType, settings.NumberOfInstances, stoppingToken);

                    longRunningJobs.AddRange(tasks);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                logger.UnexpectedError(ex);
            }

            var jobRunner = _serviceProvider.GetRequiredService<JobRunner>();

            var limit = 1; //TODO
            var listOfTasks = new List<Task>();

            await foreach (var job in infinityJobsIteration.GetJobType(_jobLaunchSettings, stoppingToken))
            {
                var toRemove = listOfTasks.Where(x => x.IsCompleted).ToList();

                foreach (var x in toRemove) 
                {
                    listOfTasks.Remove(x);
                }

                if (listOfTasks.Count >= limit)
                {
                    try
                    {
                        await Task.WhenAny(listOfTasks);
                    }
                    finally { }
                }

                var task = jobRunner.Run(job, stoppingToken);

                listOfTasks.Add(task);
            }

            await Task.WhenAll(longRunningJobs);
        }
    }
}
