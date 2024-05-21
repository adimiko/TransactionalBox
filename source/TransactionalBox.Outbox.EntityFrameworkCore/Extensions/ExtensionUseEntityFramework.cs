using Microsoft.EntityFrameworkCore;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Storage;
using TransactionalBox.Outbox.EntityFrameworkCore.Internals;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Internals.Storage.ContractsToImplement;

namespace TransactionalBox
{
    public static class ExtensionUseEntityFramework
    {
        public static void UseEntityFramework<TDbContext>(this IOutboxStorageConfigurator outboxStorageConfigurator)
            where TDbContext : DbContext
        {
            var services = outboxStorageConfigurator.Services;

            services.AddScoped<DbContext>(x => x.GetRequiredService<TDbContext>());

            services.AddScoped<IEntityFrameworkOutboxUnitOfWork, EntityFrameworkUnitOfWork>();

            services.AddScoped<IOutboxStorage, EntityFrameworkOutboxStorage>();

            services.AddScoped<IAddMessagesToTransportRepository, EntityFrameworkAddMessagesToTransportRepository>();
            services.AddScoped<ICleanUpOutboxRepository, EntityFrameworkCleanUpOutboxRepository>();

            services.AddDistributedLock<OutboxDistributedLock>(x => x.UseEntityFrameworkCore());

            services.AddSingleton<IStorageProvider, EntityFrameworkStorageProvider>();
        }
    }
}
