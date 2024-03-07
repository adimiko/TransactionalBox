using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.OutboxBase.DependencyBuilder;
using TransactionalBox.OutboxWorker.Configurators;
using TransactionalBox.OutboxWorker.Internals;

namespace TransactionalBox.OutboxWorker
{
    public static class Extensions
    {
        public static void WithWorker(
            this IOutboxDependencyBuilder builder,
            Action<IOutboxWorkerStorageConfigurator> storageConfiguration,
            Action<IOutboxWorkerTransportConfigurator> transportConfiguration)
        {
            var services = builder.Services;

            var storage = new OutboxWorkerStorageConfigurator(services);
            var transport = new OutboxWorkerTransportConfigurator(services);

            storageConfiguration(storage);
            transportConfiguration(transport);

            services.AddHostedService<OutboxProcessor>();
        }
    }
}
