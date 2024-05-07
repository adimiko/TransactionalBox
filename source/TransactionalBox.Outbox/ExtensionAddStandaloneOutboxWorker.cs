using TransactionalBox.Builders;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Configurators;
using TransactionalBox.Outbox.Internals.Extensions;
using TransactionalBox.Outbox.Settings.OutboxWorker;

namespace TransactionalBox.Outbox
{
    public static class ExtensionAddStandaloneOutboxWorker
    {
        public static void AddStandaloneOutboxWorker(
            this ITransactionalBoxBuilder builder,
            Action<IOutboxStorageConfigurator>? storageConfiguration = null,
            Action<IOutboxWorkerTransportConfigurator>? transportConfiguration = null,
            Action<OutboxWorkerSettings>? settingsConfiguration = null)
        {
            var services = builder.Services;

            var storage = new OutboxStorageConfigurator(services);

            if (storageConfiguration is not null)
            {
                storageConfiguration(storage);
            }
            else
            {
                storage.UseInMemoryStorage();
            }

            services.AddInternalOutboxWorker(transportConfiguration, settingsConfiguration);
        }
    }
}
