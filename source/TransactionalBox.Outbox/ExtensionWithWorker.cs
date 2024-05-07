using TransactionalBox.Outbox.Builders;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Settings.OutboxWorker;
using TransactionalBox.Outbox.Internals.Extensions;

namespace TransactionalBox.Outbox
{
    public static class ExtensionWithWorker
    {
        public static void WithWorker(
            this IOutboxDependencyBuilder builder,
            Action<IOutboxWorkerTransportConfigurator>? transportConfiguration = null,
            Action<OutboxWorkerSettings>? settingsConfiguration = null)
        {
            var services = builder.Services;

            services.AddInternalOutboxWorker(transportConfiguration, settingsConfiguration);
        }
    }
}
