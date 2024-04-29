using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Storage.InMemory.Internals;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.KeyedInMemoryLock;
using TransactionalBox.Inbox.Internals.Contracts;

namespace TransactionalBox.Inbox.Storage.InMemory
{
    public static class Extensions
    {
        public static void UseInMemory(this IInboxStorageConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddKeyedInMemoryLock();
            services.AddSingleton<IInboxStorage, InMemoryInboxStorage>();
            services.AddSingleton<IInboxWorkerStorage, InMemoryInboxStorage>();
            services.AddSingleton<IInboxStorageReadOnly, InMemoryInboxStorage>();
        }
    }
}
