using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals.Contracts;
using TransactionalBox.InboxWorker.Configurators;
using TransactionalBox.InboxWorker.Internals.Contracts;
using TransactionalBox.KeyedInMemoryLock;

namespace TransactionalBox.Base.Inbox.Storage.InMemory.Internals
{
    internal static class Extensions
    {
        internal static void UseInternalInMemory(this IInboxStorageConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddKeyedInMemoryLock();
            services.AddSingleton<IInboxStorage, InMemoryInboxStorage>();
            services.AddSingleton<IInboxStorageReadOnly, InMemoryInboxStorage>();
        }

        internal static void UseInternalInMemory(this IInboxWorkerStorageConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddKeyedInMemoryLock();
            services.AddSingleton<IInboxWorkerStorage, InMemoryInboxStorage>();
            services.AddSingleton<IInboxStorageReadOnly, InMemoryInboxStorage>();
        }
    }
}
