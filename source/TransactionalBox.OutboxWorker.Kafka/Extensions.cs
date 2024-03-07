using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using TransactionalBox.OutboxWorker.Configurators;
using TransactionalBox.OutboxWorker.Internals;
using TransactionalBox.OutboxWorker.Kafka.Internals;

namespace TransactionalBox.OutboxWorker.Kafka
{
    public static class Extensions
    {
        public static void UseKafka(
            this IOutboxWorkerTransportConfigurator outboxWorkerTransportConfigurator,
            string bootstrapServers)
        {
            var services = outboxWorkerTransportConfigurator.Services;

            ProducerConfig config = new ProducerConfig()
            {
                BootstrapServers = bootstrapServers,
                ClientId = Dns.GetHostName(), // TODO (HostNameService) #25
            };

            services.AddSingleton(config);

            services.AddScoped<ITransport, KafkaTransport>();
        }
    }
}
