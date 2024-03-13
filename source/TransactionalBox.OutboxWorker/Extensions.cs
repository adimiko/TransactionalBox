using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.OutboxBase.DependencyBuilder;
using TransactionalBox.OutboxWorker.Configurators;
using TransactionalBox.OutboxWorker.Internals;
using TransactionalBox.OutboxWorker.Settings;

namespace TransactionalBox.OutboxWorker
{
    public static class Extensions
    {
        public static void WithWorker(
            this IOutboxDependencyBuilder builder,
            Action<IOutboxWorkerStorageConfigurator> storageConfiguration,
            Action<IOutboxWorkerTransportConfigurator> transportConfiguration,
            Action<OutboxWorkerSettings>? settingsConfiguration = null)
        {
            var services = builder.Services;

            var storage = new OutboxWorkerStorageConfigurator(services);
            var transport = new OutboxWorkerTransportConfigurator(services);
            var settings = new OutboxWorkerSettings();
            
            storageConfiguration(storage);
            transportConfiguration(transport);

            if (settingsConfiguration is not null) 
            {
                settingsConfiguration(settings);
            }

            services.AddSingleton<IOutboxWorkerSettings>(settings);

            services.AddHostedService<OutboxProcessor>();
        }
    }
}
