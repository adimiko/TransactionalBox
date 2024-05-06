using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Base.BackgroundService.Internals;
using TransactionalBox.Base.BackgroundService.Internals.Contexts.Environment;
using TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution;
using TransactionalBox.Base.BackgroundService.Internals.JobExecutors;
using TransactionalBox.Base.BackgroundService.Internals.JobExecutors.Loggers;
using TransactionalBox.Base.BackgroundService.Internals.Launchers;
using TransactionalBox.Base.BackgroundService.Internals.Loggers;
using TransactionalBox.Base.BackgroundService.Internals.Runners;
using TransactionalBox.Base.BackgroundService.Internals.Throttling;

namespace TransactionalBox.Base.BackgroundService
{
    public static class Extensions
    {
        public static IServiceCollection AddBackgroundServiceBase(this IServiceCollection services) 
        {
            services.AddSingleton<JobIdGenerator>();
            services.AddSingleton<LongRunningJobExecutor>();
            services.AddSingleton<JobExecutor>();
            services.AddSingleton<JobRunner>();
            services.AddSingleton<ILongRunningJobRunner, LongRunningJobRunner>();
            services.AddSingleton<IEvenDistributionOfJobsFactory, EvenDistributionOfJobsFactory>();
            services.AddSingleton<IInfinityJobsIteration, InfinityJobsIteration>();

            services.AddScoped<JobExecutionContext>();
            services.AddScoped<IJobExecutionContext>(sp => sp.GetRequiredService<JobExecutionContext>());
            services.AddScoped<IJobExecutionContextConstructor>(sp => sp.GetRequiredService<JobExecutionContext>());


            services.AddSingleton(typeof(IJobExecutorLogger<>), typeof(JobExecutorLogger<>));
            services.AddSingleton(typeof(ILauncherLogger<>), typeof(LauncherLogger<>));

            services.AddSingleton<IEnvironmentContext, EnvironmentContext>();

            return services;
        }
    }
}
