using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Internals.DistributedLock;
using TransactionalBox.Internals.DistributedLock.EntityFrameworkCore;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals.Storage;
using TransactionalBox.Inbox.EntityFrameworkCore.Internals;

namespace TransactionalBox.Inbox
{
    public static class ExtensionUseEntityFramework
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
    }
}
