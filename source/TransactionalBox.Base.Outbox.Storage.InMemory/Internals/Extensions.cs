using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.KeyedInMemoryLock;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals;
using TransactionalBox.Outbox.Internals.Contracts;

namespace TransactionalBox.Base.Outbox.Storage.InMemory.Internals
{
    internal static class Extensions
    {
        internal static void UseInternalInMemoryStorage(this IOutboxStorageConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddKeyedInMemoryLock();
            services.AddSingleton<IOutboxStorageReadOnly, InMemoryOutboxStorage>();
            services.AddSingleton<IOutboxStorage, InMemoryOutboxStorage>();
            services.AddSingleton<IOutboxWorkerStorage, InMemoryOutboxStorage>();
        }
    }
}
