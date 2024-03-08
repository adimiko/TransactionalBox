using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.InboxWorker.Configurators;
using TransactionalBox.InboxWorker.Internals;
using TransactionalBox.InboxWorker.Kafka.Internals;

namespace TransactionalBox.InboxWorker.Kafka
{
    public static class Extensions
    {
        public static void UseKafka(
            this IInboxWorkerTransportConfigurator inboxWorkerTransportConfigurator,
            string bootstrapServers)
        {
            var services = inboxWorkerTransportConfigurator.Services;

            var config = new ConsumerConfig()
            {
                GroupId = "ModuleName", //TODO ServiceNameProvider #28
                BootstrapServers = bootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
            };

            services.AddSingleton(config);

            services.AddSingleton<IInboxWorkerTransport, KafkaInboxWorkerTransport>();
        }
    }
}
