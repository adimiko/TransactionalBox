using Microsoft.Extensions.DependencyInjection;


namespace TransactionalBox.Internals.InternalPackages.KeyedInMemoryLock
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
