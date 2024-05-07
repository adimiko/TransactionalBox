using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Builders;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Configurators;
using TransactionalBox.Outbox.Settings.Outbox;
using TransactionalBox.Outbox.Builders;
using TransactionalBox.Outbox.Internals.Builders;
using TransactionalBox.Outbox.Internals.Oubox;
using TransactionalBox.Outbox.Internals.Extensions;

namespace TransactionalBox.Outbox
{
    public static class ExtensionAddOutbox
    {
        public static IOutboxDependencyBuilder AddOutbox(
            this ITransactionalBoxBuilder builder,
            Action<IOutboxStorageConfigurator>? storageConfiguration = null,
            Action<OutboxSettings>? configureSettings = null)
        {
            var services = builder.Services;

            var storage = new OutboxStorageConfigurator(services);
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

            if (configureSettings is not null)
            {
                configureSettings(settings);
            }

            settings.Configure(serialization);

            services.AddScoped<IOutbox, InternalOutbox>();

            return new OutboxDependencyBuilder(services);
        }


    }
}
