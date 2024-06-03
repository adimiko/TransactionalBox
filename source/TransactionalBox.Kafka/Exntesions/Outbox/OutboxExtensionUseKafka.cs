using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Configurators.Outbox;
using TransactionalBox.Internals.Outbox.Transport.ContractsToImplement;
using TransactionalBox.Kafka.Internals.Outbox;
using TransactionalBox.Kafka.Internals.Outbox.ImplementedContracts;
using TransactionalBox.Kafka.Settings.Outbox;

namespace TransactionalBox
{
    public static class OutboxExtensionUseKafka
    {
        public static void UseKafka(
            this IOutboxTransportConfigurator outboxWorkerTransportConfigurator,
            Action<OutboxKafkaSettings> settingsConfiguration)
        {
            var services = outboxWorkerTransportConfigurator.Services;
            var settings = new OutboxKafkaSettings();

            settingsConfiguration(settings);

            services.AddSingleton<IOutboxKafkaSettings>(settings);
            services.AddSingleton<ITransportMessageSizeSettings>(settings.TransportMessageSizeSettings);

            services.AddSingleton<KafkaConfigFactory>();
            services.AddScoped<IOutboxTransport, KafkaOutboxTransport>();
        }
    }
}
