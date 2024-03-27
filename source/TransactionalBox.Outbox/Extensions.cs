using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Builders;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals;
using TransactionalBox.Outbox.Internals.Configurators;
using TransactionalBox.Outbox.Internals.Serializers;
using TransactionalBox.Outbox.Serialization;
using TransactionalBox.Outbox.Settings;
using TransactionalBox.OutboxBase.DependencyBuilder;

namespace TransactionalBox.Outbox
{
    public static class Extensions
    {
        public static IOutboxDependencyBuilder AddOutbox(
            this ITransactionalBoxBuilder builder,
            Action<IOutboxStorageConfigurator> configureStorage,
            Action<OutboxSettings>? configureSettings = null)
        {
            var services = builder.Services;

            var storage = new OutboxStorageConfigurator(services);
            var settings = new OutboxSettings();

            var serialization = new OutboxSerializationConfigurator(services);

            configureStorage(storage);

            if (configureSettings is not null)
            {
                configureSettings(settings);
            }

            settings.Configure(serialization);

            services.AddSingleton<TopicFactory>();
            services.AddScoped<IOutbox, InternalOutbox>();

            return new OutboxDependencyBuilder(services);
        }

        public static void UseSystemTextJson(
            this IOutboxSerializationConfigurator configurator) 
        {
            var services = configurator.Services;

            services.AddSingleton<IOutboxSerializer, OutboxSerializer>();
        }
    }
}
