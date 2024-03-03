using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.EntityFramework.Internals;
using TransactionalBox.Outbox.Internals;

namespace TransactionalBox.Outbox.EntityFramework
{
    public static class Extensions
    {
        public static IOutboxStorageConfigurator UseEntityFramework<TDbContext>(this IOutboxStorageConfigurator outboxStorageConfigurator)
            where TDbContext : DbContext
        {
            var services = outboxStorageConfigurator.Services;

            services.AddScoped<IOutboxStorage, EntityFrameworkOutboxStorage>();

            services.AddScoped<DbContext>(x => x.GetRequiredService<TDbContext>());
            return outboxStorageConfigurator;
        }

        public static ModelBuilder AddOutbox(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());

            return modelBuilder;
        }
    }
}
