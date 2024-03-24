using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.BackgroundServiceBase.Internals;
using TransactionalBox.BackgroundServiceBase.Internals.Context;
using TransactionalBox.BackgroundServiceBase.Internals.Loggers;

namespace TransactionalBox.BackgroundServiceBase
{
    public static class Extensions
    {
        public static IServiceCollection AddBackgroundServiceBase(this IServiceCollection services) 
        {
            services.AddSingleton<JobIdGenerator>();
            services.AddSingleton<JobExecutor>();
            services.AddSingleton<IParallelExecutor, ParallelExecutor>();

            services.AddScoped<JobExecutionContext>();
            services.AddScoped<IJobExecutionContext>(sp => sp.GetRequiredService<JobExecutionContext>());
            services.AddScoped<IJobExecutionContextConstructor>(sp => sp.GetRequiredService<JobExecutionContext>());

            services.AddSingleton(typeof(IJobExecutorLogger<>), typeof(JobExecutorLogger<>));
            services.AddSingleton(typeof(ILauncherLogger<>), typeof(LauncherLogger<>));

            return services;
        }
    }
}
