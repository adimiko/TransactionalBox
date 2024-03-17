using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.OutboxWorker.Configurators;
using TransactionalBox.OutboxWorker.EntityFramework.Internals;
using TransactionalBox.OutboxWorker.Internals;

namespace TransactionalBox.OutboxWorker.EntityFramework
{
    public static class Extensions
    {
        public static void UseEntityFramework(this IOutboxWorkerStorageConfigurator outboxWorkerStorageConfigurator)
        {
            var services = outboxWorkerStorageConfigurator.Services;

            services.AddScoped<IOutboxStorage, EntityFrameworkOutboxStorage>();
            services.AddScoped<EntityFrameworkOutboxDistributedLockStorage>();
        }
    }
}
