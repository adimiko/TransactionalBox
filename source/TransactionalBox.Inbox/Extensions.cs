﻿using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Builders;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Deserialization;
using TransactionalBox.Inbox.Internals;
using TransactionalBox.Inbox.Internals.Configurators;
using TransactionalBox.Inbox.Internals.Deserializers;
using TransactionalBox.Inbox.Settings;
using TransactionalBox.InboxBase.DependencyBuilder;

namespace TransactionalBox.Inbox
{
    public static class Extensions
    {
        public static IInboxDependencyBuilder AddInbox(
            this ITransactionalBoxBuilder builder,
            Action<IInboxStorageConfigurator> storageConfiguration,
            Action<InboxSettings>? configureSettings = null)
        {
            var services = builder.Services;

            var storage = new InboxStorageConfigurator(services);
            var serialization = new InboxDeserializationConfigurator(services);
            var settings = new InboxSettings();

            storageConfiguration(storage);

            if (configureSettings is not null) 
            {
                configureSettings(settings);
            }

            settings.Configure(serialization);

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var allTypes = assemblies.SelectMany(x => x.GetTypes());

            var inboxMessageHandlerTypes = allTypes
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IInboxMessageHandler<>)))
            .ToList();

            services.Scan(s =>
            s.FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(IInboxMessageHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.AddSingleton<IInboxMessageTypes>(new InboxMessageTypes(inboxMessageHandlerTypes));
            services.AddHostedService<InboxProcessor>();

            return new InboxDependencyBuilder(services);
        }

        internal static void UseSystemTextJson(this IInboxDeserializationConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<IInboxDeserializer, InboxDeserializer>();
        }
    }
}
