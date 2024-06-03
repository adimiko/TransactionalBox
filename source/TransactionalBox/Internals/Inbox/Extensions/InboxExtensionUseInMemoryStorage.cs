using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Internals.Inbox.Storage.ContractsToImplement;
using TransactionalBox.Internals.Inbox.Storage.InMemory;
using TransactionalBox.Configurators.Inbox;

namespace TransactionalBox.Internals.Inbox.Extensions
{
    internal static class InboxExtensionUseInMemoryStorage
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
