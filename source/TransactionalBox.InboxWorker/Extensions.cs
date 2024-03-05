using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.InboxBase.DependencyBuilder;
using TransactionalBox.InboxWorker.Configurators;
using TransactionalBox.InboxWorker.Internals;

namespace TransactionalBox.InboxWorker
{
    public static class Extensions
    {
        public static IInboxDependencyBuilder WithWorker(
            this IInboxDependencyBuilder builder,
            Action<IInboxWorkerStorageConfigurator> storageConfiguration,
            Action<IInboxWorkerTransportConfigurator> transportConfiguration)
        {
            var services = builder.Services;

            var storage = new InboxWorkerStorageConfigurator(services);
            var transport = new InboxWorkerTransportConfigurator(services);

            storageConfiguration(storage);
            transportConfiguration(transport);

            services.AddHostedService<InboxTransportProcessor>();

            return builder;
        }
    }
}
