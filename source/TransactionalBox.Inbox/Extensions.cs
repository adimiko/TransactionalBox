using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Builders;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Deserialization;
using TransactionalBox.Inbox.Internals;
using TransactionalBox.Inbox.Internals.Configurators;
using TransactionalBox.Inbox.Internals.Deserializers;
using TransactionalBox.Inbox.Internals.Jobs;
using TransactionalBox.Inbox.Settings;
using TransactionalBox.Base.Inbox.DependencyBuilder;
using TransactionalBox.Base.BackgroundService;
using TransactionalBox.Inbox.Internals.Launchers;
using TransactionalBox.Base.Inbox.MessageTypesFromAssemblies.Internals;
using TransactionalBox.Inbox.Internals.Contexts;
using TransactionalBox.Inbox.Internals.Topics;

namespace TransactionalBox.Inbox
{
    public static class Extensions
    {
        public static IInboxDependencyBuilder AddInbox(
            this ITransactionalBoxBuilder builder,
            Action<IInboxStorageConfigurator> storageConfiguration, //TODO null ?
            Action<IInboxTransportConfigurator> transportConfiguration, //TODO null ?
            Action<InboxSettings>? configureSettings = null,
            Action<IInboxAssemblyConfigurator>? assemblyConfiguraton = null)
        {
            var services = builder.Services;

            var storage = new InboxStorageConfigurator(services);
            var transport = new InboxTransportConfigurator(services);
            var serialization = new InboxDeserializationConfigurator(services);
            var decompression = new InboxDecompressionAlgorithmConfigurator(services);
            var assemblyConfigurator = new InboxAssemblyConfigurator();
            var settings = new InboxSettings();

            storageConfiguration(storage);
            transportConfiguration(transport);

            if (assemblyConfiguraton is not null)
            {
                assemblyConfiguraton(assemblyConfigurator);
            }

            if (configureSettings is not null) 
            {
                configureSettings(settings);
            }

            settings.Configure(serialization, decompression);

            services.AddSingleton<IInboxLauncherSettings>(settings);
            services.AddSingleton<IProcessMessageFromInboxJobSettings>(settings);

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

            //TODO register topics service, and messages (lisen event from another services)
            services.AddSingleton<ITopicsProvider, TopicsProvider>();

            //TODO
            services.AddSingleton<IInboxWorkerContext, InboxWorkerContext>();

            // Settings
            services.AddSingleton<IAddMessagesToInboxStorageJobSettings>(settings.AddMessagesToInboxStorageSettings);
            services.AddSingleton<IAddMessagesToInboxStorageLauncherSettings>(settings.AddMessagesToInboxStorageSettings);

            services.AddSingleton<ICleanUpProcessedInboxMessagesJobSettings>(settings.CleanUpProcessedInboxMessagesSettings);
            services.AddSingleton<ICleanUpProcessedInboxMessagesLauncherSettings>(settings.CleanUpProcessedInboxMessagesSettings);

            services.AddSingleton<ICleanUpExpiredIdempotencyKeysJobSettings>(settings.CleanUpExpiredIdempotencyKeysSettings);
            services.AddSingleton<ICleanUpExpiredIdempotencyKeysLauncherSettings>(settings.CleanUpExpiredIdempotencyKeysSettings);

            // Jobs
            services.AddHostedService<InboxLauncher>();

            services.AddScoped<ProcessMessageFromInbox>();
            services.AddScoped<AddMessagesToInboxStorage>();
            services.AddScoped<CleanUpProcessedInboxMessages>();
            services.AddScoped<CleanUpExpiredIdempotencyKeys>();

            return new InboxDependencyBuilder(services);
        }

        internal static void UseSystemTextJson(this IInboxDeserializationConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<IInboxDeserializer, InboxDeserializer>();
        }
    }
}
