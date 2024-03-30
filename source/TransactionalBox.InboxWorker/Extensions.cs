using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.BackgroundServiceBase;
using TransactionalBox.InboxBase.DependencyBuilder;
using TransactionalBox.InboxWorker.Configurators;
using TransactionalBox.InboxWorker.Decompression;
using TransactionalBox.InboxWorker.Internals.Configurators;
using TransactionalBox.InboxWorker.Internals.Contexts;
using TransactionalBox.InboxWorker.Internals.Decompression;
using TransactionalBox.InboxWorker.Internals.Jobs;
using TransactionalBox.InboxWorker.Internals.Launchers;
using TransactionalBox.InboxWorker.Internals.Services;
using TransactionalBox.InboxWorker.Settings;

namespace TransactionalBox.InboxWorker
{
    public static class Extensions
    {
        public static void WithWorker(
            this IInboxDependencyBuilder builder,
            Action<IInboxWorkerStorageConfigurator> storageConfiguration,
            Action<IInboxWorkerTransportConfigurator> transportConfiguration,
            Action<InboxWorkerSettings>? settingsConfiguration = null)
        {
            var services = builder.Services;

            var storage = new InboxWorkerStorageConfigurator(services);
            var transport = new InboxWorkerTransportConfigurator(services);
            var settings = new InboxWorkerSettings();

            storageConfiguration(storage);
            transportConfiguration(transport);

            if (settingsConfiguration is not null)
            {
                settingsConfiguration(settings);
            }

            var decompression = new InboxWorkerDecompressionAlgorithmConfigurator(services);

            settings.Configure(decompression);

            services.AddBackgroundServiceBase();

            services.AddSingleton<IAddMessagesToInboxStorageLauncherSettings>(settings.AddMessagesToInboxStorageSettings);

            services.AddHostedService<InboxWorkerLauncher>();
            services.AddScoped<AddMessagesToInboxStorage>();

            services.AddSingleton<IInboxWorkerContext,InboxWorkerContext>();

            //TODO register topics service, and messages (lisen event from another services)
            services.AddSingleton<ITopicsProvider, TopicsProvider>();
        }
    }
}
