using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.InboxWorker.Configurators;
using TransactionalBox.InboxWorker.EntityFramework.Internals;
using TransactionalBox.InboxWorker.Internals.Contracts;

namespace TransactionalBox.InboxWorker.EntityFramework
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
