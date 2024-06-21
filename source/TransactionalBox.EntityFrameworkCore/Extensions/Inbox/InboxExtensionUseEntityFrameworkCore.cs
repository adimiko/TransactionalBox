using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.EntityFrameworkCore.Internals.Inbox.ImplementedContracts;
using TransactionalBox.Configurators.Inbox;
using TransactionalBox.Internals.Inbox.Storage;
using TransactionalBox.Internals.Inbox.Storage.ContractsToImplement;
using TransactionalBox.Internals.InternalPackages.DistributedLock;
using TransactionalBox.EntityFrameworkCore.Internals.InternalPackages.DistributedLock;

namespace TransactionalBox
{
    public static class InboxExtensionUseEntityFrameworkCore
    {
        public static void UseEntityFrameworkCore<TDbContext>(this IInboxStorageConfigurator storageConfigurator)
            where TDbContext : DbContext
        {
            var services = storageConfigurator.Services;

            services.AddScoped<DbContext>(x => x.GetRequiredService<TDbContext>());
            services.AddScoped<IAddMessagesToInboxRepository, EntityFrameworkCoreAddMessagesToInboxRepository>();
            services.AddScoped<ICleanUpIdempotencyKeysRepository, EntityFrameworkCoreCleanUpIdempotencyKeysRepository>();
            services.AddScoped<IProcessMessageRepository, EntityFrameworkCoreProcessMessageRepository>();
            services.AddScoped<ICleanUpInboxRepository, EntityFrameworkCoreCleanUpInboxRepository>();

            services.AddDistributedLock<InboxDistributedLock>(x => x.UseEntityFrameworkCore<TDbContext>());
        }
    }
}
