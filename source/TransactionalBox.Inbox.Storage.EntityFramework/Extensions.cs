using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Storage.EntityFramework.Internals;
using TransactionalBox.Base.Inbox.StorageModel.EntityFramework.Internals;
using TransactionalBox.Base.Inbox.StorageModel.Internals;
using TransactionalBox.DistributedLock;
using TransactionalBox.DistributedLock.EntityFramework;
using TransactionalBox.Inbox.Internals.Contracts;

namespace TransactionalBox.Inbox.Storage.EntityFramework
{
    public static class Extensions
    {
        public static void UseEntityFramework<TDbContext>(this IInboxStorageConfigurator storageConfigurator)
            where TDbContext : DbContext
        {
            var services = storageConfigurator.Services;

            services.AddScoped<DbContext>(x => x.GetRequiredService<TDbContext>());
            services.AddScoped<IInboxStorage, EntityFrameworkInboxStorage>();
            services.AddScoped<IInboxWorkerStorage, EntityFrameworkInboxWorkerStorage>();

            services.AddDistributedLock<InboxDistributedLock>(x => x.UseEntityFramework<TDbContext>());
        }

        public static void AddInbox(this ModelBuilder modelBuilder)
        {
            modelBuilder.AddInboxStorageModel();
        }
    }
}
