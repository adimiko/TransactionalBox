using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Configurators;
using TransactionalBox.OutboxWorker.Configurators;
using TransactionalBox.OutboxWorker.Internals;

namespace TransactionalBox.OutboxWorker
{
    public static class Extensions
    {
        public static ITransactionalBoxConfigurator UseOutboxWorker(
            this ITransactionalBoxConfigurator configurator,
            Action<IOutboxWorkerStorageConfigurator> storageConfiguration,
            Action<IOutboxWorkerTransportConfigurator> transportConfiguration)
        {
            var services = configurator.Services;

            services.AddHostedService<OutboxProcessor>();

            return configurator;
        }
    }
}
