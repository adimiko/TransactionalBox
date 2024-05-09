﻿using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Builders;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Configurators;
using TransactionalBox.Outbox.Internals.Oubox;
using TransactionalBox.Outbox.Internals.Extensions;
using TransactionalBox.Base.BackgroundService;
using TransactionalBox.Outbox.Internals.Jobs.AddMessagesToTransportJob.TransportMessageFactories.Policies;
using TransactionalBox.Outbox.Internals.Jobs.AddMessagesToTransportJob.TransportMessageFactories;
using TransactionalBox.Outbox.Internals.Jobs.AddMessagesToTransportJob;
using TransactionalBox.Outbox.Internals.Jobs;
using TransactionalBox.Outbox.Internals.Launchers.Settings;
using TransactionalBox.Outbox.Internals.Launchers;
using TransactionalBox.Outbox.Internals.Loggers;
using TransactionalBox.Outbox.Settings;

namespace TransactionalBox.Outbox
{
    public static class ExtensionAddOutbox
    {
        public static void AddOutbox(
            this ITransactionalBoxBuilder builder,
            Action<IOutboxStorageConfigurator>? storageConfiguration = null,
            Action<IOutboxTransportConfigurator>? transportConfiguration = null,
            Action<OutboxSettings>? settingsConfiguration = null)
        {
            var services = builder.Services;

            var storage = new OutboxStorageConfigurator(services);
            var transport = new OutboxTransportConfigurator(services);
            var settings = new OutboxSettings();

            var serialization = new OutboxSerializationConfigurator(services);

            if (storageConfiguration is not null)
            {
                storageConfiguration(storage);
            }
            else
            {
                storage.UseInMemoryStorage();
            }

            if (settingsConfiguration is not null)
            {
                settingsConfiguration(settings);
            }

            settings.Configure(serialization);

            if (transportConfiguration is not null)
            {
                transportConfiguration(transport);
            }
            else
            {
                transport.UseInMemoryTransport();
            }

            var compressionAlgorithm = new OutboxCompressionAlgorithmConfigurator(services);

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

            services.AddScoped<IOutbox, InternalOutbox>();
        }
    }
}
