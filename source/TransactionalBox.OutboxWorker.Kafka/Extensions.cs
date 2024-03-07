using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using TransactionalBox.OutboxWorker.Configurators;
using TransactionalBox.OutboxWorker.Internals;
using TransactionalBox.OutboxWorker.Kafka.Internals;
using TransactionalBox.OutboxWorker.Kafka.Settings;

namespace TransactionalBox.OutboxWorker.Kafka
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

            ProducerConfig config = new ProducerConfig()
            {
                BootstrapServers = settings.BootstrapServers,
                ClientId = Dns.GetHostName(), // TODO (HostNameService) #25
            };

            services.AddSingleton(config);

            services.AddScoped<ITransport, KafkaTransport>();
        }
    }
}
