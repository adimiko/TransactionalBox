using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Internals.KeyedInMemoryLock.Internals;

namespace TransactionalBox.Internals.KeyedInMemoryLock
{
    internal static class ExtensionAddKeyedInMemoryLock
    {
        internal static IServiceCollection AddKeyedInMemoryLock(this IServiceCollection services)
        {
            services.AddSingleton<IKeyedInMemoryLock, InternalKeyedInMemoryLock>();

            return services;
        }
    }
}
