using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Internals.Inbox.Storage.ContractsToImplement;
using TransactionalBox.Internals.Inbox.Storage.InMemory;
using TransactionalBox.Configurators.Inbox;
using TransactionalBox.Internals.InternalPackages.KeyedInMemoryLock;

namespace TransactionalBox.Internals.Inbox.Extensions
{
    internal static class InboxExtensionUseInMemoryStorage
    {
        internal static void UseInMemoryStorage(this IInboxStorageConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddKeyedInMemoryLock();
            services.AddSingleton<IAddMessagesToInboxRepository, InMemoryInboxStorage>();
            services.AddSingleton<ICleanUpIdempotencyKeysRepository, InMemoryInboxStorage>();
            services.AddSingleton<ICleanUpInboxRepository, InMemoryInboxStorage>();
            services.AddSingleton<IProcessMessageRepository, InMemoryInboxStorage>();
            services.AddSingleton<IInboxStorageReadOnly, InMemoryInboxStorage>();
        }
    }
}
