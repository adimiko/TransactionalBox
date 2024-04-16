using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.OutboxWorker.Configurators;
using TransactionalBox.OutboxWorker.Internals.Contracts;
using TransactionalBox.OutboxWorker.Transport.Kafka.Internals;
using TransactionalBox.OutboxWorker.Transport.Kafka.Settings;

namespace TransactionalBox.OutboxWorker.Transport.Kafka
{
    public static class Extensions
    {
        public static void UseKafka(
            this IOutboxWorkerTransportConfigurator outboxWorkerTransportConfigurator,
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
