using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Builders;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals;
using TransactionalBox.Outbox.Internals.Configurators;
using TransactionalBox.Outbox.Settings.Outbox;
using TransactionalBox.Outbox.Settings.OutboxWorker;
using TransactionalBox.Base.BackgroundService;
using TransactionalBox.Outbox.Internals.Loggers;
using TransactionalBox.Outbox.Internals.Jobs.AddMessagesToTransportJob.TransportMessageFactories;
using TransactionalBox.Outbox.Internals.Jobs.AddMessagesToTransportJob;
using TransactionalBox.Outbox.Internals.Launchers;
using TransactionalBox.Outbox.Internals.Jobs;
using TransactionalBox.Outbox.Internals.Jobs.AddMessagesToTransportJob.TransportMessageFactories.Policies;
using TransactionalBox.Outbox.Builders;
using TransactionalBox.Outbox.Internals.Builders;
using TransactionalBox.Outbox.Internals.Storage.InMemory;
using TransactionalBox.Outbox.Internals.Transport.InMemory;

namespace TransactionalBox.Outbox
{
    public static class Extensions
    {
        public static IOutboxDependencyBuilder AddOutbox(
            this ITransactionalBoxBuilder builder,
            Action<IOutboxStorageConfigurator>? storageConfiguration = null,
            Action<OutboxSettings>? configureSettings = null)
        {
            var services = builder.Services;

            var storage = new OutboxStorageConfigurator(services);
            var settings = new OutboxSettings();

            var serialization = new OutboxSerializationConfigurator(services);

            if (storageConfiguration is not null)
            {
                storageConfiguration(storage);
            }
            else
            {
                storage.UseInternalInMemory();
            }

            if (configureSettings is not null)
            {
                configureSettings(settings);
            }

            settings.Configure(serialization);

            services.AddSingleton<TopicFactory>();
            services.AddScoped<IOutbox, InternalOutbox>();

            return new OutboxDependencyBuilder(services);
        }

        public static void WithWorker(
            this IOutboxDependencyBuilder builder,
            Action<IOutboxWorkerTransportConfigurator>? transportConfiguration = null,
            Action<OutboxWorkerSettings>? settingsConfiguration = null)
        {
            var services = builder.Services;

            var transport = new OutboxWorkerTransportConfigurator(services);
            var settings = new OutboxWorkerSettings();

            if (transportConfiguration is not null) 
            {
                transportConfiguration(transport);
            }
            else
            {
                transport.UseInternalInMemory();
            }

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

            // Policies
            services.AddSingleton<IPayloadCreationPolicy, PayloadHasOptimalSizePolicy>();
            services.AddSingleton<IPayloadCreationPolicy, PayloadIsLargerThanOptimalSizePolicy>();
        }
    }
}
