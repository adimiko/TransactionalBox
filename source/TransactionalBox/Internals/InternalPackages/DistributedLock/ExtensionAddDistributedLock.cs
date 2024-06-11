using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Internals.InternalPackages.KeyedInMemoryLock;

namespace TransactionalBox.Internals.InternalPackages.DistributedLock
{
    internal static class ExtensionAddDistributedLock
    {
        internal static void AddDistributedLock<T>(
            this IServiceCollection services,
            Action<IDistributedLockStorageConfigurator> storageConfiguration)
            where T : Lock, new()
        {
            var storage = new DistributedLockStorageConfigurator(services);

            storageConfiguration(storage);

            services.AddKeyedInMemoryLock();
            services.AddSingleton<IDistributedLock<T>, InternalDistributedLock<T>>();
        }
    }
}
