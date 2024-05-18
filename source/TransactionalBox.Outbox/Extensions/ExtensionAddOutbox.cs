using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Builders;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Configurators;
using TransactionalBox.Outbox.Internals.Oubox;
using TransactionalBox.Outbox.Internals.Extensions;
using TransactionalBox.Outbox.Settings;
using TransactionalBox.Internals.EventHooks;
using TransactionalBox.Outbox.Internals.Storage;
using TransactionalBox.Outbox.Internals.Hooks.Events;
using TransactionalBox.Outbox.Internals.Hooks.Handlers.CleanUpOutbox;
using TransactionalBox.Outbox.Internals.Hooks.Handlers.AddMessagesToTransport;
using TransactionalBox.Outbox.Internals.Hooks.Handlers.AddMessagesToTransport.TransportMessageFactories;
using TransactionalBox.Outbox.Internals.Hooks.Handlers.AddMessagesToTransport.TransportMessageFactories.Policies;
using TransactionalBox.Outbox.Internals.Hooks.Handlers.CleanUpOutbox.Loggers;
using TransactionalBox.Outbox.Internals.Hooks.Handlers.AddMessagesToTransport.Loggers;
using TransactionalBox.Outbox.Internals.Hooks.Handlers.AddMessagesToTransport.Logger;
using TransactionalBox.Outbox.Internals.Hooks.Handlers.CleanUpOutbox.Logger;

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

            var compression = new OutboxCompressionConfigurator(services);

            settings.Configure(compression);

            services.AddSingleton<TransportMessageFactory>();

            // Settings
            services.AddSingleton<IAddMessagesToTransportSettings>(settings.AddMessagesToTransportSettings);

            services.AddSingleton<ICleanUpOutboxSettings>(settings.CleanUpOutboxSettings);

            // Hooks
            services.AddEventHookHandler<AddMessagesToTransport, AddedMessagesToOutbox>();

            if (settings.CleanUpOutboxSettings.IsEnabled)
            {
                services.AddEventHookHandler<CleanUpOutbox, AddedMessagesToTransport>();
            }

            // Policies
            services.AddSingleton<IPayloadCreationPolicy, PayloadHasOptimalSizePolicy>();
            services.AddSingleton<IPayloadCreationPolicy, PayloadIsLargerThanOptimalSizePolicy>();

            services.AddScoped<IOutbox, Internals.Oubox.Outbox>();

            services.AddSingleton<ITranactionCommited, TranactionCommited>();

            // Loggers
            services.AddSingleton<ICleanUpOutboxLogger, CleanUpOutboxLogger>();
            services.AddSingleton<IAddMessagesToTransportLogger, AddMessagesToTransportLogger>();
        }
    }
}
