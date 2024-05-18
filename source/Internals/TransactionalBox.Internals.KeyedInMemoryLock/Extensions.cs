using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Internals.KeyedInMemoryLock.Internals;

namespace TransactionalBox.Internals.KeyedInMemoryLock
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
