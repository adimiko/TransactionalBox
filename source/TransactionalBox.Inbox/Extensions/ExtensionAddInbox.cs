using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Builders;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals.Configurators;
using TransactionalBox.Inbox.Internals.Contexts;
using TransactionalBox.Inbox.Internals.Transport.Topics;
using TransactionalBox.Inbox.Internals.Assemblies.MessageTypes;
using TransactionalBox.Inbox.Internals.Assemblies.CompiledHandlers;
using TransactionalBox.Inbox.Settings;
using TransactionalBox.Internals.EventHooks;
using TransactionalBox.Inbox.Internals.Hooks.Events;
using TransactionalBox.Inbox.Internals.Hooks.Handlers.CleanUpInbox;
using TransactionalBox.Inbox.Internals.Hooks.Handlers.ProcessMessage;
using TransactionalBox.Inbox.Internals.BackgroundProcesses.AddMessagesToInbox;
using TransactionalBox.Inbox.Internals.BackgroundProcesses.CleanUpIdempotencyKeys;
using TransactionalBox.Inbox.Internals.Extensions;
using TransactionalBox.Inbox.Internals.Hooks.Handlers.CleanUpInbox.Logger;
using TransactionalBox.Inbox.Internals.Hooks.Handlers.ProcessMessage.Logger;
using TransactionalBox.Inbox.Internals.BackgroundProcesses.AddMessagesToInbox.Logger;
using TransactionalBox.Inbox.Internals.BackgroundProcesses.CleanUpIdempotencyKeys.Logger;
using TransactionalBox.Inbox.Internals;
using TransactionalBox.Inbox.Internals.BackgroundProcesses.Base;
using TransactionalBox.Inbox.Internals.InboxMessageDefinitions;
using TransactionalBox.Inbox;
using TransactionalBox.Inbox.Internals.Decompression;
using Microsoft.IO;

namespace TransactionalBox
{
    public static class ExtensionAddInbox
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
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IInboxMessageHandler<>)))
            .ToList();

            services.Scan(s =>
            s.FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(IInboxMessageHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            var inboxMessageDefinitions = allTypes.Where(x => x.BaseType != null && x.BaseType.IsGenericType && x.BaseType.GetGenericTypeDefinition() == typeof(InboxMessageDefinition<>)).ToList();

            foreach (var inboxMessageDefinition in inboxMessageDefinitions)
            {
                var messageType = inboxMessageDefinition.BaseType.GetGenericArguments()[0];

                services.AddKeyedSingleton(typeof(IInboxMessageDefinition), messageType, inboxMessageDefinition);
            }

            services.AddSingleton<IInboxMessageTypes>(new InboxMessageTypes(inboxMessageHandlerTypes, typeof(IInboxMessageHandler<>)));
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
