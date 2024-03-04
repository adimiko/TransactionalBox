using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Configurators;
using TransactionalBox.InboxWorker.Configurators;
using TransactionalBox.InboxWorker.Internals;

namespace TransactionalBox.InboxWorker
{
    public static class Extensions
    {
        public static ITransactionalBoxConfigurator AddInboxWorker(
            this ITransactionalBoxConfigurator transactionalBoxConfigurator,
            Action<IInboxWorkerStorageConfigurator> storageConfiguration,
            Action<IInboxWorkerTransportConfigurator> transportConfiguration)
        {
            var services = transactionalBoxConfigurator.Services;

            var storage = new InboxWorkerStorageConfigurator(services);
            var transport = new InboxWorkerTransportConfigurator(services);

            storageConfiguration(storage);
            transportConfiguration(transport);

            services.AddHostedService<InboxTransportProcessor>();

            return transactionalBoxConfigurator;
        }
    }
}
