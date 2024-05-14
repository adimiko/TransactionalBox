using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals.Storage.InMemory;
using TransactionalBox.Inbox.Internals.Storage;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.KeyedInMemoryLock;

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
