using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Builders;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals;
using TransactionalBox.Inbox.Internals.Configurators;
using TransactionalBox.Inbox.Internals.Jobs;
using TransactionalBox.Base.BackgroundService;
using TransactionalBox.Inbox.Internals.Contexts;
using TransactionalBox.Inbox.Internals.Topics;
using TransactionalBox.Inbox.Builders;
using TransactionalBox.Inbox.Internals.Builders;
using TransactionalBox.Inbox.Settings.InboxWorker;
using TransactionalBox.Inbox.Settings.Inbox;
using TransactionalBox.Inbox.Internals.Launchers.Inbox;
using TransactionalBox.Inbox.Internals.Launchers.InboxWorker;

namespace TransactionalBox.Inbox
{
    public static class Extensions
    {
        public static IInboxDependencyBuilder AddInbox(
            this ITransactionalBoxBuilder builder,
            Action<IInboxStorageConfigurator> storageConfiguration, //TODO null ?
            Action<InboxSettings>? settingsConfiguration = null,
            Action<IInboxAssemblyConfigurator>? assemblyConfiguraton = null)
        {
            var services = builder.Services;

            var storage = new InboxStorageConfigurator(services);
            
            var serialization = new InboxDeserializationConfigurator(services);
            var assemblyConfigurator = new InboxAssemblyConfigurator();
            var settings = new InboxSettings();

            storageConfiguration(storage);

            if (assemblyConfiguraton is not null)
            {
                assemblyConfiguraton(assemblyConfigurator);
            }

            if (settingsConfiguration is not null) 
            {
                settingsConfiguration(settings);
            }

            settings.Configure(serialization);

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

            // Jobs
            services.AddHostedService<InboxLauncher>();

            services.AddScoped<ProcessMessageFromInbox>();

            return new InboxDependencyBuilder(services);
        }

        public static void WithWorker(
            this IInboxDependencyBuilder builder,
            Action<IInboxTransportConfigurator> transportConfiguration,
            Action<InboxWorkerSettings>? settingsConfiguration = null)
        {
            var services = builder.Services;

            var settings = new InboxWorkerSettings();
            var transport = new InboxTransportConfigurator(services);
            var decompressionConfigurator = new InboxDecompressionAlgorithmConfigurator(services);

            if (settingsConfiguration is not null)
            {
                settingsConfiguration(settings);
            }

            transportConfiguration(transport);

            settings.Configure(decompressionConfigurator);

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
            services.AddHostedService<InboxWorkerLauncher>();

            services.AddScoped<AddMessagesToInboxStorage>();
            services.AddScoped<CleanUpProcessedInboxMessages>();
            services.AddScoped<CleanUpExpiredIdempotencyKeys>();
        }
    }
}
