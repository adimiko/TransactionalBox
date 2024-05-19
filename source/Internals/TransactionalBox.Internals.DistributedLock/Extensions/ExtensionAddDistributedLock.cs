using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Internals.DistributedLock.Configurators;
using TransactionalBox.Internals.DistributedLock.Internals;
using TransactionalBox.Internals.DistributedLock.Internals.Configurators;
using TransactionalBox.Internals.KeyedInMemoryLock;

namespace TransactionalBox.Internals.DistributedLock.Extensions
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
