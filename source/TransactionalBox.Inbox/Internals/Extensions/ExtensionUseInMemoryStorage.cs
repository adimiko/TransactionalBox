using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals.Storage.InMemory;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Internals.KeyedInMemoryLock;
using TransactionalBox.Inbox.Internals.Storage.ContractsToImplement;

namespace TransactionalBox.Inbox.Internals.Extensions
{
    internal static class ExtensionUseInMemoryStorage
    {
        internal static void UseInMemoryStorage(this IInboxStorageConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddKeyedInMemoryLock();
            services.AddSingleton<IInboxStorage, InMemoryInboxStorage>();
            services.AddSingleton<IInboxWorkerStorage, InMemoryInboxStorage>();
            services.AddSingleton<IInboxStorageReadOnly, InMemoryInboxStorage>();
        }
    }
}
