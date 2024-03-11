using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.EntityFramework.Internals;
using TransactionalBox.Outbox.Internals;
using TransactionalBox.OutboxBase.StorageModel.EntityFramework.Internals;

namespace TransactionalBox.Outbox.EntityFramework
{
    public static class Extensions
    {
        public static void UseEntityFramework<TDbContext>(this IOutboxStorageConfigurator outboxStorageConfigurator)
            where TDbContext : DbContext
        {
            var services = outboxStorageConfigurator.Services;

            services.AddScoped<IOutboxStorage, EntityFrameworkOutboxStorage>();

            services.AddScoped<DbContext>(x => x.GetRequiredService<TDbContext>());
        }

        public static void AddOutbox(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        }
    }
}
