using TransactionalBox.Builders;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Configurators;
using TransactionalBox.Outbox.Internals.Storage.InMemory;
using TransactionalBox.Outbox.Settings.OutboxWorker;

namespace TransactionalBox.Outbox
{
    public static class ExtensionAddOutboxWorker
    {
        public static void AddOutboxWorker(
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
                storage.UseInternalInMemory();
            }

            services.AddInternalOutboxWorker(transportConfiguration, settingsConfiguration);
        }
    }
}
