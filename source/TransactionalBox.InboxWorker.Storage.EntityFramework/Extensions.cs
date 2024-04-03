using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.InboxWorker.Configurators;
using TransactionalBox.InboxWorker.Storage.EntityFramework.Internals;
using TransactionalBox.InboxWorker.Internals.Contracts;

namespace TransactionalBox.InboxWorker.Storage.EntityFramework
{
    public static class Extensions
    {
        public static void UseEntityFramework(this IInboxWorkerStorageConfigurator inboxWorkerStorageConfigurator)
        {
            var services = inboxWorkerStorageConfigurator.Services;

            services.AddScoped<IInboxStorage, EntityFrameworkInboxStorage>();
        }
    }
}
