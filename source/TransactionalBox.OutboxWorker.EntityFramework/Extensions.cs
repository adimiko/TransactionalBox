using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.DistributedLock;
using TransactionalBox.DistributedLock.EntityFramework;
using TransactionalBox.OutboxBase.StorageModel.Internals;
using TransactionalBox.OutboxWorker.Configurators;
using TransactionalBox.OutboxWorker.EntityFramework.Internals;
using TransactionalBox.OutboxWorker.Internals.Contracts;

namespace TransactionalBox.OutboxWorker.EntityFramework
{
    public static class Extensions
    {
        public static void UseEntityFramework(this IOutboxWorkerStorageConfigurator outboxWorkerStorageConfigurator)
        {
            var services = outboxWorkerStorageConfigurator.Services;

            services.AddScoped<IOutboxStorage, EntityFrameworkOutboxStorage>();

            services.AddDistributedLock<OutboxDistributedLock>(x => x.UseEntityFramework());
        }
    }
}
