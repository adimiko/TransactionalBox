using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.BackgroundServiceBase.Internals;

namespace TransactionalBox.BackgroundServiceBase
{
    public static class Extensions
    {
        public static IServiceCollection AddBackgroundServiceBase(this IServiceCollection services) 
        {
            services.AddSingleton<JobIdGenerator>();
            services.AddSingleton<JobExecutor>();
            services.AddSingleton<IParallelExecutor, ParallelExecutor>();

            return services;
        }
    }
}
