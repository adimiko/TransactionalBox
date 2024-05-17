using Microsoft.EntityFrameworkCore;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Storage;
using TransactionalBox.Outbox.Storage.EntityFramework.Internals.EntityTypeConfigurations;
using TransactionalBox.Outbox.Storage.EntityFramework.Internals;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.DistributedLock;
using TransactionalBox.DistributedLock.EntityFramework;

namespace TransactionalBox.Outbox
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

            services.AddDistributedLock<OutboxDistributedLock>(x => x.UseEntityFramework());
        }
    }
}
