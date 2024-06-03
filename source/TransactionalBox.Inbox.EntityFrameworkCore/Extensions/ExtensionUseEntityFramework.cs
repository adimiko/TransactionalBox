using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals.Storage;
using TransactionalBox.Inbox.Internals.Storage.ContractsToImplement;
using TransactionalBox.Inbox.EntityFrameworkCore.Internals.ImplementedContracts;

namespace TransactionalBox
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

            services.AddDistributedLock<InboxDistributedLock>(x => x.UseEntityFrameworkCore<TDbContext>());
        }
    }
}
