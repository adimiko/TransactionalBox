using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Transport;
using TransactionalBox.Outbox.Transport.Kafka.Internals;
using TransactionalBox.Outbox.Transport.Kafka.Settings;

namespace TransactionalBox.Outbox.Transport.Kafka
{
    public static class Extensions
    {
        public static void UseKafka(
            this IOutboxTransportConfigurator outboxWorkerTransportConfigurator,
            Action<OutboxWorkerKafkaSettings> settingsConfiguration)
        {
            var services = outboxWorkerTransportConfigurator.Services;
            var settings = new OutboxWorkerKafkaSettings();

            settingsConfiguration(settings);

            services.AddSingleton<IOutboxWorkerKafkaSettings>(settings);
            services.AddSingleton<ITransportMessageSizeSettings>(settings.TransportMessageSizeSettings);

            services.AddSingleton<KafkaConfigFactory>();
            services.AddScoped<IOutboxWorkerTransport, KafkaTransport>();
        }
    }
}
