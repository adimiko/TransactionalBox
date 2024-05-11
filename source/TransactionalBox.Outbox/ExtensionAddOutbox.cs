using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Builders;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Configurators;
using TransactionalBox.Outbox.Internals.Oubox;
using TransactionalBox.Outbox.Internals.Extensions;
using TransactionalBox.Outbox.Internals.Loggers;
using TransactionalBox.Outbox.Settings;
using TransactionalBox.Base.EventHooks;
using TransactionalBox.Outbox.Internals.Hooks.AddMessagesToTransport;
using TransactionalBox.Outbox.Internals.Hooks.AddMessagesToTransport.TransportMessageFactories;
using TransactionalBox.Outbox.Internals.Hooks.AddMessagesToTransport.TransportMessageFactories.Policies;
using TransactionalBox.Outbox.Internals.Hooks.CleanUpProcessedOutboxMessages;
using TransactionalBox.Outbox.Internals.Hooks.EventHooks;
using TransactionalBox.Outbox.Internals.Storage;

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

            services.AddSingleton(typeof(IOutboxWorkerLogger<>), typeof(OutboxWorkerLogger<>));
            services.AddSingleton<TransportMessageFactory>();

            // Settings
            services.AddSingleton<IAddMessagesToTransportHookSettings>(settings.AddMessagesToTransportSettings);

            services.AddSingleton<ICleanUpProcessedOutboxMessagesHookSettings>(settings.CleanUpProcessedOutboxMessagesSettings);

            // Hooks
            services.AddEventHookHandler<AddMessagesToTransport, AddedMessagesToOutboxEventHook>();

            if (settings.CleanUpProcessedOutboxMessagesSettings.IsEnabled)
            {
                services.AddEventHookHandler<CleanUpProcessedOutboxMessages, AddedMessagesToTransportEventHook>();
            }

            // Policies
            services.AddSingleton<IPayloadCreationPolicy, PayloadHasOptimalSizePolicy>();
            services.AddSingleton<IPayloadCreationPolicy, PayloadIsLargerThanOptimalSizePolicy>();

            services.AddScoped<IOutbox, InternalOutbox>();

            services.AddSingleton<ITranactionCommited, TranactionCommited>();
        }
    }
}
