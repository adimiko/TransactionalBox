using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Configurators;
using TransactionalBox.OutboxWorker.Configurators;
using TransactionalBox.OutboxWorker.Internals;

namespace TransactionalBox.OutboxWorker
{
    public static class Extensions
    {
        public static ITransactionalBoxConfigurator AddOutboxWorker(
            this ITransactionalBoxConfigurator configurator,
            Action<IOutboxWorkerStorageConfigurator> storageConfiguration,
            Action<IOutboxWorkerTransportConfigurator> transportConfiguration)
        {
            var services = configurator.Services;

            var storage = new OutboxWorkerStorageConfigurator(services);
            var transport = new OutboxWorkerTransportConfigurator(services);

            storageConfiguration(storage);
            transportConfiguration(transport);

            services.AddHostedService<OutboxProcessor>();

            return configurator;
        }
    }
}
