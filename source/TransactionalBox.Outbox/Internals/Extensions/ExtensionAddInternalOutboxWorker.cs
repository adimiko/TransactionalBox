using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Configurators;
using TransactionalBox.Outbox.Internals.Jobs.AddMessagesToTransportJob.TransportMessageFactories.Policies;
using TransactionalBox.Outbox.Internals.Jobs.AddMessagesToTransportJob.TransportMessageFactories;
using TransactionalBox.Outbox.Internals.Jobs.AddMessagesToTransportJob;
using TransactionalBox.Outbox.Internals.Jobs;
using TransactionalBox.Outbox.Internals.Launchers.Settings;
using TransactionalBox.Outbox.Internals.Launchers;
using TransactionalBox.Outbox.Internals.Loggers;
using TransactionalBox.Outbox.Settings.OutboxWorker;
using TransactionalBox.Outbox.Internals.Transport.InMemory;
using TransactionalBox.Base.BackgroundService;

namespace TransactionalBox.Outbox.Internals.Extensions
{
    internal static class ExtensionAddInternalOutboxWorker
    {
        internal static void AddInternalOutboxWorker(
            this IServiceCollection services,
            Action<IOutboxWorkerTransportConfigurator>? transportConfiguration = null,
            Action<OutboxWorkerSettings>? settingsConfiguration = null)
        {
            var transport = new OutboxWorkerTransportConfigurator(services);
            var settings = new OutboxWorkerSettings();

            if (transportConfiguration is not null)
            {
                transportConfiguration(transport);
            }
            else
            {
                transport.UseInMemoryTransport();
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
