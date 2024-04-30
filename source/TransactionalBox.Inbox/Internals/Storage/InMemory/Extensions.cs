using TransactionalBox.Inbox.Configurators;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.KeyedInMemoryLock;
using TransactionalBox.Inbox.Internals.Contracts;

namespace TransactionalBox.Inbox.Internals.Storage.InMemory
{
    internal static class Extensions
    {
        internal static void UseInternalInMemory(this IInboxStorageConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddKeyedInMemoryLock();
            services.AddSingleton<IInboxStorage, InMemoryInboxStorage>();
            services.AddSingleton<IInboxWorkerStorage, InMemoryInboxStorage>();
            services.AddSingleton<IInboxStorageReadOnly, InMemoryInboxStorage>();
        }
    }
}
