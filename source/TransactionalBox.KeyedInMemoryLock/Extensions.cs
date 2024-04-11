using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.KeyedInMemoryLock.Internals;

namespace TransactionalBox.KeyedInMemoryLock
{
    public static class Extensions
    {
        public static IServiceCollection AddKeyedInMemoryLock(this IServiceCollection services)
        {
            services.AddSingleton<IKeyedInMemoryLock, InternalKeyedInMemoryLock>();

            return services;
        }
    }
}
