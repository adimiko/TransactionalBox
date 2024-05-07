using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Storage.InMemory;
using TransactionalBox.Outbox.Internals.Storage;
using TransactionalBox.KeyedInMemoryLock;
using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Outbox.Internals.Extensions
{
    internal static class ExtensionUseInMemoryStorage
    {
        internal static void UseInMemoryStorage(this IOutboxStorageConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddKeyedInMemoryLock();
            services.AddSingleton<IOutboxStorageReadOnly, InMemoryOutboxStorage>();
            services.AddSingleton<IOutboxStorage, InMemoryOutboxStorage>();
            services.AddSingleton<IOutboxWorkerStorage, InMemoryOutboxStorage>();
        }
    }
}
