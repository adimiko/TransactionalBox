using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.KeyedInMemoryLock;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals;
using TransactionalBox.Outbox.Internals.Contracts;
using TransactionalBox.Outbox.Storage.InMemory.Internals;

namespace TransactionalBox.Outbox.Storage.InMemory
{
    public static class Extensions
    {
        public static void UseInMemory(this IOutboxStorageConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddKeyedInMemoryLock();
            services.AddSingleton<IOutboxStorageReadOnly, InMemoryOutboxStorage>();
            services.AddSingleton<IOutboxStorage, InMemoryOutboxStorage>();
            services.AddSingleton<IOutboxWorkerStorage, InMemoryOutboxStorage>();
        }
    }
}
