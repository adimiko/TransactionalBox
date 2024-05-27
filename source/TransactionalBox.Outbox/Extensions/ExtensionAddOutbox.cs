using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Builders;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Configurators;
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
using TransactionalBox.Outbox.Internals.OutboxMessageDefinitions;
using TransactionalBox.Outbox;
using TransactionalBox.Internals.SequentialGuid;
using TransactionalBox.Internals.SequentialGuid.Internals;

namespace TransactionalBox
{
    public static class ExtensionAddOutbox
    {
        public static void AddOutbox(
            this ITransactionalBoxBuilder builder,
            Action<IOutboxStorageConfigurator>? storageConfiguration = null,
            Action<IOutboxTransportConfigurator>? transportConfiguration = null,
            Action<OutboxSettings>? settingsConfiguration = null,
            Action<IOutboxAssemblyConfigurator>? assemblyConfiguraton = null)
        {
            var services = builder.Services;

            var storage = new OutboxStorageConfigurator(services);
            var transport = new OutboxTransportConfigurator(services);
            var settings = new OutboxSettings();

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

            settings.ConfigureDelegates(services);

            if (transportConfiguration is not null)
            {
                transportConfiguration(transport);
            }
            else
            {
                transport.UseInMemoryTransport();
            }

            // Assembly
            var assemblyConfigurator = new OutboxAssemblyConfigurator();

            if (assemblyConfiguraton is not null)
            {
                assemblyConfiguraton(assemblyConfigurator);
            }

            var assemblies = assemblyConfigurator.Assemblies;

            var allTypes = assemblyConfigurator.Assemblies.SelectMany(x => x.GetTypes());

            var outboxMessageDefinitions = allTypes.Where(x => x.BaseType != null && x.BaseType.IsGenericType && x.BaseType.GetGenericTypeDefinition() == typeof(OutboxMessageDefinition<>)).ToList();

            foreach (var outboxMessageDefinition in outboxMessageDefinitions)
            {
                var messageType = outboxMessageDefinition.BaseType.GetGenericArguments()[0];

                services.AddKeyedSingleton(typeof(IOutboxMessageDefinition), messageType, outboxMessageDefinition);
            }

            services.AddSingleton<TransportEnvelopeFactory>();

            services.AddSingleton<SequentialGuidConfigurator>();
            services.AddSingleton<ISequentialGuidGenerator>(sp => new SequentialGuid(sp.GetRequiredService<SequentialGuidConfigurator>().Create()));

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

            services.AddScoped<IOutbox, Outbox.Internals.Oubox.Outbox>();

            services.AddSingleton<ITranactionCommited, TranactionCommited>();

            // Loggers
            services.AddSingleton<ICleanUpOutboxLogger, CleanUpOutboxLogger>();
            services.AddSingleton<IAddMessagesToTransportLogger, AddMessagesToTransportLogger>();
        }
    }
}
