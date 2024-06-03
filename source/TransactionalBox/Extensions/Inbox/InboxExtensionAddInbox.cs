using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Builders;
using Microsoft.IO;
using TransactionalBox.Internals.Inbox;
using TransactionalBox.Internals.Inbox.Extensions;
using TransactionalBox.Internals.Inbox.Assemblies.CompiledHandlers;
using TransactionalBox.Internals.Inbox.Assemblies.MessageTypes;
using TransactionalBox.Internals.Inbox.Contexts;
using TransactionalBox.Internals.Inbox.Decompression;
using TransactionalBox.Internals.Inbox.InboxMessageDefinitions;
using TransactionalBox.Internals.Inbox.BackgroundProcesses.AddMessagesToInbox;
using TransactionalBox.Internals.Inbox.BackgroundProcesses.AddMessagesToInbox.Logger;
using TransactionalBox.Internals.Inbox.BackgroundProcesses.Base;
using TransactionalBox.Internals.Inbox.BackgroundProcesses.CleanUpIdempotencyKeys;
using TransactionalBox.Internals.Inbox.BackgroundProcesses.CleanUpIdempotencyKeys.Logger;
using TransactionalBox.Internals.Inbox.Hooks.Events;
using TransactionalBox.Internals.Inbox.Hooks.Handlers.CleanUpInbox;
using TransactionalBox.Internals.Inbox.Hooks.Handlers.CleanUpInbox.Logger;
using TransactionalBox.Internals.Inbox.Hooks.Handlers.ProcessMessage;
using TransactionalBox.Internals.Inbox.Hooks.Handlers.ProcessMessage.Logger;
using TransactionalBox.Internals.Inbox.Transport.Topics;
using TransactionalBox.Configurators.Inbox;
using TransactionalBox.Settings.Inbox;
using TransactionalBox.Internals.Inbox.Configurators;

namespace TransactionalBox
{
    public static class InboxExtensionAddInbox
    {
        public static void AddInbox(
            this ITransactionalBoxBuilder builder,
            Action<IInboxStorageConfigurator>? storageConfiguration = null,
            Action<IInboxTransportConfigurator>? transportConfiguration = null,
            Action<InboxSettings>? settingsConfiguration = null,
            Action<IInboxAssemblyConfigurator>? assemblyConfiguraton = null)
        {
            var services = builder.Services;

            // Storage
            var storage = new InboxStorageConfigurator(services);

            if (storageConfiguration is not null)
            {
                storageConfiguration(storage);
            }
            else
            {
                storage.UseInMemoryStorage();
            }

            // Transport
            var transport = new InboxTransportConfigurator(services);

            if (transportConfiguration is not null)
            {
                transportConfiguration(transport);
            }
            else
            {
                transport.UseInMemoryTransport();
            }

            // Settings
            var settings = new InboxSettings();

            if (settingsConfiguration is not null)
            {
                settingsConfiguration(settings);
            }

            settings.ConfigureDelegates(services);

            // Assembly
            var assemblyConfigurator = new InboxAssemblyConfigurator();

            if (assemblyConfiguraton is not null)
            {
                assemblyConfiguraton(assemblyConfigurator);
            }

            var assemblies = assemblyConfigurator.Assemblies;

            var allTypes = assemblyConfigurator.Assemblies.SelectMany(x => x.GetTypes());

            var inboxMessageHandlerTypes = allTypes
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IInboxHandler<>)))
            .ToList();

            services.Scan(s =>
            s.FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(IInboxHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            var inboxMessageDefinitions = allTypes.Where(x => x.BaseType != null && x.BaseType.IsGenericType && x.BaseType.GetGenericTypeDefinition() == typeof(InboxMessageDefinition<>)).ToList();

            foreach (var inboxMessageDefinition in inboxMessageDefinitions)
            {
                var messageType = inboxMessageDefinition.BaseType.GetGenericArguments()[0];

                services.AddKeyedSingleton(typeof(IInboxMessageDefinition), messageType, inboxMessageDefinition);
            }

            services.AddSingleton<IInboxMessageTypes>(new InboxMessageTypes(inboxMessageHandlerTypes, typeof(IInboxHandler<>)));
            services.AddSingleton<ICompiledInboxHandlers, CompiledInboxHandlers>();

            services.AddSingleton<ITopicsProvider, TopicsProvider>();

            services.AddSingleton<IInboxContext, InboxContext>();

            // Settings
            services.AddSingleton<IAddMessagesToInboxSettings>(settings.AddMessagesToInboxSettings);

            services.AddSingleton<ICleanUpInboxSettings>(settings.CleanUpInboxSettings);

            services.AddSingleton<ICleanUpIdempotencyKeysSettings>(settings.CleanUpIdempotencyKeysSettings);

            services.AddHostedService<InboxStartup>();
            services.AddEventHookHandler<ProcessMessage, AddedMessagesToInbox>();

            if (settings.CleanUpInboxSettings.IsEnabled)
            {
                services.AddEventHookHandler<CleanUpInbox, ProcessedMessageFromInbox>();
            }
           
            // Loggers
            services.AddSingleton<ICleanUpInboxLogger, CleanUpInboxLogger>();
            services.AddSingleton<IProcessMessageLogger, ProcessMessageLogger>();
            services.AddSingleton<IAddMessagesToInboxLogger, AddMessagesToInboxLogger>();
            services.AddSingleton<ICleanUpIdempotencyKeysLogger, CleanUpIdempotencyKeysLogger>();


            // BackgroundProcesses
            services.AddSingleton<BackgroundProcessBase, AddMessagesToInbox>();
            services.AddSingleton<BackgroundProcessBase, CleanUpIdempotencyKeys>();

            // Decompression
            services.AddSingleton<IDecompressionFactory, DecompressionFactory>();

            services.AddSingleton(new RecyclableMemoryStreamManager());
            services.AddSingleton<IDecompression, BrotliDecompression>();
            services.AddSingleton<IDecompression, GZipDecompression>();
            services.AddSingleton<IDecompression, NoDecompression>();
        }
    }
}
