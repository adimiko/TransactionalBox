using Microsoft.EntityFrameworkCore;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Storage;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Internals.Storage.ContractsToImplement;
using TransactionalBox.Outbox.EntityFrameworkCore.Internals.ImplementedContracts;

namespace TransactionalBox
{
    public static class ExtensionUseEntityFramework
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
