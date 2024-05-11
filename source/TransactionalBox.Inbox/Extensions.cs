using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Builders;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals.Configurators;
using TransactionalBox.Inbox.Internals.Jobs;
using TransactionalBox.Base.BackgroundService;
using TransactionalBox.Inbox.Internals.Contexts;
using TransactionalBox.Inbox.Internals.Storage.InMemory;
using TransactionalBox.Inbox.Internals.Transport.InMemory;
using TransactionalBox.Inbox.Internals.Transport.Topics;
using TransactionalBox.Inbox.Internals.Assemblies.MessageTypes;
using TransactionalBox.Inbox.Internals.Assemblies.CompiledHandlers;
using TransactionalBox.Inbox.Settings;
using TransactionalBox.Inbox.Internals.Launchers;

namespace TransactionalBox.Inbox
{
    public static class Extensions
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
                storage.UseInternalInMemory();
            }

            // Transport
            var transport = new InboxTransportConfigurator(services);

            if (transportConfiguration is not null)
            {
                transportConfiguration(transport);
            }
            else
            {
                transport.UseInternalInMemory();
            }

            // Settings
            var settings = new InboxSettings();

            if (settingsConfiguration is not null)
            {
                settingsConfiguration(settings);
            }

            // Serialization
            var serialization = new InboxDeserializationConfigurator(services);

            settings.Configure(serialization);

            // Decompression
            var decompressionConfigurator = new InboxDecompressionAlgorithmConfigurator(services);

            settings.Configure(decompressionConfigurator);

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


            services.AddBackgroundServiceBase();

            services.AddSingleton<IInboxMessageTypes>(new InboxMessageTypes(inboxMessageHandlerTypes, typeof(IInboxMessageHandler<>)));
            services.AddSingleton<ICompiledInboxHandlers, CompiledInboxHandlers>();

            // Launchers
            services.AddHostedService<InboxLauncher>();

            services.AddSingleton<ITopicsProvider, TopicsProvider>();

            services.AddSingleton<IInboxContext, InboxContext>();

            // Job Settings
            services.AddSingleton<IProcessingMessagesFromInboxLauncherSettings>(settings.ProcessingMessagesFromInboxSettings);
            services.AddSingleton<IProcessMessageFromInboxJobSettings>(settings.ProcessingMessagesFromInboxSettings);

            services.AddSingleton<IAddMessagesToInboxStorageJobSettings>(settings.AddMessagesToInboxStorageSettings);

            services.AddSingleton<ICleanUpProcessedInboxMessagesJobSettings>(settings.CleanUpProcessedInboxMessagesSettings);
            services.AddSingleton<ICleanUpProcessedInboxMessagesLauncherSettings>(settings.CleanUpProcessedInboxMessagesSettings);

            services.AddSingleton<ICleanUpExpiredIdempotencyKeysJobSettings>(settings.CleanUpExpiredIdempotencyKeysSettings);
            services.AddSingleton<ICleanUpExpiredIdempotencyKeysLauncherSettings>(settings.CleanUpExpiredIdempotencyKeysSettings);

            // Jobs
            services.AddHostedService<AddMessagesToInboxStorage>();
            services.AddScoped<ProcessMessageFromInbox>();
            services.AddScoped<AddMessagesToInboxStorage>();
            services.AddScoped<CleanUpProcessedInboxMessages>();
            services.AddScoped<CleanUpExpiredIdempotencyKeys>();
        }
    }
}
