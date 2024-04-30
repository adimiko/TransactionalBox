using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Storage.EntityFramework.Internals;
using TransactionalBox.Outbox.Internals;
using TransactionalBox.Outbox.Internals.Contracts;
using TransactionalBox.DistributedLock.EntityFramework;
using TransactionalBox.DistributedLock;
using TransactionalBox.Outbox.Storage.EntityFramework.Internals.EntityTypeConfigurations;
using TransactionalBox.Outbox.Internals.Storage;

namespace TransactionalBox.Outbox.Storage.EntityFramework
{
    public static class Extensions
    {
        public static void UseEntityFramework<TDbContext>(this IOutboxStorageConfigurator outboxStorageConfigurator)
            where TDbContext : DbContext
        {
            var services = outboxStorageConfigurator.Services;

            services.AddScoped<DbContext>(x => x.GetRequiredService<TDbContext>());

            services.AddScoped<IOutboxStorage, EntityFrameworkOutboxStorage>();

            services.AddScoped<IOutboxWorkerStorage, EntityFrameworkOutboxWorkerStorage>();

            services.AddDistributedLock<OutboxDistributedLock>(x => x.UseEntityFramework());
        }

        public static void AddOutbox(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
            modelBuilder.AddDistributedLock<OutboxDistributedLock>();
        }
    }
}
