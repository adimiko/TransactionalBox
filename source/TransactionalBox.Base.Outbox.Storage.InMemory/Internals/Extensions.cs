using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals;
using TransactionalBox.OutboxWorker.Configurators;
using TransactionalBox.OutboxWorker.Internals.Contracts;

namespace TransactionalBox.Base.Outbox.Storage.InMemory.Internals
{
    internal static class Extensions
    {
        internal static void UseInternalInMemoryStorage(this IOutboxStorageConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<IOutboxStorageReadOnly, InMemoryOutboxStorage>();
            services.AddSingleton<IOutboxStorage, InMemoryOutboxStorage>();
        }

        internal static void UseInternalInMemoryStorage(this IOutboxWorkerStorageConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<IOutboxStorageReadOnly, InMemoryOutboxStorage>();
            services.AddSingleton<IOutboxWorkerStorage, InMemoryOutboxStorage>();
        }
    }
}
