using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.EntityFrameworkCore.Internals.Outbox.ImplementedContracts;
using TransactionalBox.Configurators.Outbox;
using TransactionalBox.Internals.Outbox.Storage.ContractsToImplement;
using TransactionalBox.Internals.Outbox.Storage;
using TransactionalBox.Internals.InternalPackages.DistributedLock;
using TransactionalBox.EntityFrameworkCore.Internals.InternalPackages.DistributedLock;

namespace TransactionalBox
{
    public static class OutboxExtensionUseEntityFramework
    {
        public static void UseEntityFramework<TDbContext>(this IOutboxStorageConfigurator outboxStorageConfigurator)
            where TDbContext : DbContext
        {
            var services = outboxStorageConfigurator.Services;

            services.AddScoped<DbContext>(x => x.GetRequiredService<TDbContext>());

            services.AddScoped<IOutboxStorage, EntityFrameworkOutboxStorage>();

            services.AddScoped<IAddMessagesToTransportRepository, EntityFrameworkAddMessagesToTransportRepository>();
            services.AddScoped<ICleanUpOutboxRepository, EntityFrameworkCleanUpOutboxRepository>();

            services.AddDistributedLock<OutboxDistributedLock>(x => x.UseEntityFrameworkCore());

            services.AddSingleton<IStorageProvider, EntityFrameworkStorageProvider>();
        }
    }
}
