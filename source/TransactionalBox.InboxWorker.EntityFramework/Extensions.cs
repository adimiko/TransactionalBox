using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.InboxWorker.Configurators;
using TransactionalBox.InboxWorker.EntityFramework.Internals;
using TransactionalBox.InboxWorker.Internals;

namespace TransactionalBox.InboxWorker.EntityFramework
{
    public static class Extensions
    {
        public static void UseEntityFramework(this IInboxWorkerStorageConfigurator inboxWorkerStorageConfigurator)
        {
            var services = inboxWorkerStorageConfigurator.Services;

            services.AddScoped<IInboxStorage, EntityFrameworkInboxStorage>();
        }

        public static ModelBuilder AddInbox(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());

            return modelBuilder;
        }
    }
}
