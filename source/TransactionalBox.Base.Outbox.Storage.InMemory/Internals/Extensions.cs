using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals;
using TransactionalBox.OutboxWorker.Configurators;

namespace TransactionalBox.Base.Outbox.Storage.InMemory.Internals
{
    internal static class Extensions
    {
        internal static void UseInternalInMemoryStorage(this IOutboxStorageConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<IOutboxStorage, InMemoryOutboxStorage>();
        }

        internal static void UseInternalInMemoryStorage(this IOutboxWorkerStorageConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<IOutboxStorage, InMemoryOutboxStorage>();
        }
    }
}
