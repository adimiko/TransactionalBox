using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Storage.EntityFramework.Internals;
using TransactionalBox.Outbox.Internals;
using TransactionalBox.Base.Outbox.StorageModel.EntityFramework.Internals;

namespace TransactionalBox.Outbox.Storage.EntityFramework
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
            modelBuilder.AddOutboxStorageModel();
        }
    }
}
