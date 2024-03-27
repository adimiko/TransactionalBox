using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.BackgroundServiceBase;
using TransactionalBox.OutboxBase.DependencyBuilder;
using TransactionalBox.OutboxWorker.Compression;
using TransactionalBox.OutboxWorker.Configurators;
using TransactionalBox.OutboxWorker.Internals;
using TransactionalBox.OutboxWorker.Internals.Compression;
using TransactionalBox.OutboxWorker.Internals.Configurators;
using TransactionalBox.OutboxWorker.Internals.Contracts;
using TransactionalBox.OutboxWorker.Internals.Jobs;
using TransactionalBox.OutboxWorker.Internals.Loggers;
using TransactionalBox.OutboxWorker.Settings;
using TransactionalBox.OutboxWorker.Settings.Compression;

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

            services.AddSingleton<IOutboxProcessorSettings>(settings);
            services.AddSingleton<IOutboxOrchestratorSettings>(settings);
            services.AddSingleton<TransportMessageFactory>();

            services.AddHostedService<OutboxWorkerLauncher>();

            services.AddScoped<AddMessagesToTransport>();
        }

        public static void UseBrotliCompression(
            this IOutboxWorkerCompressionAlgorithmConfigurator configurator,
            Action<BrotliCompressionSettings>? configureCompressionSettings = null)
        {
            var services = configurator.Services;

            var settings = new BrotliCompressionSettings();

            if (configureCompressionSettings is not null)
            {
                configureCompressionSettings(settings);
            }

            services.AddSingleton<IBrotliCompressionSettings>(settings);

            services.AddSingleton<ICompressionAlgorithm, BrotliCompression>();
        }
    }
}
