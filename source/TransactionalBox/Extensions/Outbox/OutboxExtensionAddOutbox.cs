﻿using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Builders;
using TransactionalBox.Settings.Outbox;
using TransactionalBox.Configurators.Outbox;
using TransactionalBox.Internals.Outbox.Configurators;
using TransactionalBox.Internals.Outbox.Hooks.Events;
using TransactionalBox.Internals.Outbox.Hooks.Handlers.AddMessagesToTransport;
using TransactionalBox.Internals.Outbox.Hooks.Handlers.AddMessagesToTransport.TransportMessageFactories;
using TransactionalBox.Internals.Outbox.Hooks.Handlers.AddMessagesToTransport.TransportMessageFactories.Policies;
using TransactionalBox.Internals.Outbox.Hooks.Handlers.CleanUpOutbox;
using TransactionalBox.Internals.Outbox.Hooks.Handlers.CleanUpOutbox.Logger;
using TransactionalBox.Internals.Outbox.Storage;
using TransactionalBox.Internals.Outbox.Extensions;
using TransactionalBox.Internals.Outbox.Hooks.Handlers.AddMessagesToTransport.Logger;
using TransactionalBox.Internals.Outbox;
using TransactionalBox.Internals.Outbox.OutboxDefinitions;
using TransactionalBox.Internals.InternalPackages.SequentialGuid;
using TransactionalBox.Internals.InternalPackages.EventHooks;
using TransactionalBox.Configurators;
using TransactionalBox.Internals.InternalPackages.AssemblyConfigurator;

namespace TransactionalBox
{
    public static class OutboxExtensionAddOutbox
    {
        public static void AddOutbox(
            this ITransactionalBoxBuilder builder,
            Action<IOutboxStorageConfigurator>? storageConfiguration = null,
            Action<IOutboxTransportConfigurator>? transportConfiguration = null,
            Action<OutboxSettings>? settingsConfiguration = null,
            Action<IAssemblyConfigurator>? assemblyConfiguraton = null)
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
            var assemblyConfigurator = new AssemblyConfigurator();

            if (assemblyConfiguraton is not null)
            {
                assemblyConfiguraton(assemblyConfigurator);
            }

            services.RegisterOutboxMessageDefinitions(assemblyConfigurator);

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

            services.AddScoped<IOutbox, Internals.Outbox.Oubox.Outbox>();

            // Loggers
            services.AddSingleton<ICleanUpOutboxLogger, CleanUpOutboxLogger>();
            services.AddSingleton<IAddMessagesToTransportLogger, AddMessagesToTransportLogger>();

            services.AddHostedService<OutboxStartup>();
        }

        private static void RegisterOutboxMessageDefinitions(this IServiceCollection services, AssemblyConfigurator assemblyConfigurator) 
        {
            var assemblies = assemblyConfigurator.Assemblies;

            var allTypes = assemblyConfigurator.Assemblies.SelectMany(x => x.GetTypes());

            var outboxMessageDefinitions = allTypes.Where(x => x.BaseType != null && x.BaseType.IsGenericType && x.BaseType.GetGenericTypeDefinition() == typeof(OutboxDefinition<>)).ToList();

            foreach (var outboxMessageDefinition in outboxMessageDefinitions)
            {
                var messageType = outboxMessageDefinition.BaseType.GetGenericArguments()[0];

                services.AddKeyedSingleton(typeof(IOutboxDefinition), messageType, outboxMessageDefinition);
            }
        }
    }
}
