using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Internals.Outbox.Storage.InMemory;
using TransactionalBox.Internals.Outbox.Storage.ContractsToImplement;
using TransactionalBox.Configurators.Outbox;

namespace TransactionalBox.Internals.Outbox.Extensions
{
    internal static class ExtensionUseInMemoryStorage
    {
        internal static void UseInMemoryStorage(this IOutboxStorageConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddKeyedInMemoryLock();
            services.AddSingleton<IOutboxStorageReadOnly, InMemoryOutboxStorage>();
            services.AddSingleton<IOutboxStorage, InMemoryOutboxStorage>();
            services.AddSingleton<IStorageProvider, InMemoryStorageProvider>();
            services.AddSingleton<IAddMessagesToTransportRepository, InMemoryOutboxStorage>();
            services.AddSingleton<ICleanUpOutboxRepository, InMemoryOutboxStorage>();
        }
    }
}
