using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.BackgroundServiceBase;
using TransactionalBox.Base.Outbox.DependencyBuilder;
using TransactionalBox.OutboxWorker.Compression;
using TransactionalBox.OutboxWorker.Configurators;
using TransactionalBox.OutboxWorker.Internals;
using TransactionalBox.OutboxWorker.Internals.Compression;
using TransactionalBox.OutboxWorker.Internals.Configurators;
using TransactionalBox.OutboxWorker.Internals.Jobs;
using TransactionalBox.OutboxWorker.Internals.Launchers;
using TransactionalBox.OutboxWorker.Internals.Loggers;
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

            var compressionAlgorithm = new OutboxWorkerCompressionAlgorithmConfigurator(services);

            settings.Configure(compressionAlgorithm);

            services.AddBackgroundServiceBase();

            services.AddSingleton(typeof(IOutboxWorkerLogger<>), typeof(OutboxWorkerLogger<>));
            services.AddSingleton<TransportMessageFactory>();


            // Settings
            services.AddSingleton<IAddMessagesToTransportJobSettings>(settings.AddMessagesToTransportSettings);
            services.AddSingleton<IAddMessagesToTransportLauncherSettings>(settings.AddMessagesToTransportSettings);

            services.AddSingleton<ICleanUpProcessedOutboxMessagesJobSettings>(settings.CleanUpProcessedOutboxMessagesSettings);
            services.AddSingleton<ICleanUpProcessedOutboxMessagesLauncherSettings>(settings.CleanUpProcessedOutboxMessagesSettings);

            services.AddHostedService<OutboxWorkerLauncher>();

            // Jobs
            services.AddScoped<AddMessagesToTransport>();
            services.AddScoped<CleanUpProcessedOutboxMessages>();
        }
    }
}
