using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.InboxWorker.Configurators;
using TransactionalBox.InboxWorker.Internals.Contracts;
using TransactionalBox.InboxWorker.Transport.Kafka.Internals;
using TransactionalBox.InboxWorker.Transport.Kafka.Settings;

namespace TransactionalBox.InboxWorker.Transport.Kafka
{
    public static class Extensions
    {
        public static void UseKafka(
            this IInboxWorkerTransportConfigurator inboxWorkerTransportConfigurator,
            Action<InboxWorkerKafkaSettings> settingsConfiguration = null)
        {
            var services = inboxWorkerTransportConfigurator.Services;
            var settings = new InboxWorkerKafkaSettings();

            if (settingsConfiguration is not null) 
            {
                settingsConfiguration(settings);
            }

            services.AddSingleton<IInboxWorkerKafkaSettings>(settings);
            services.AddSingleton<KafkaConfigFactory>();
            services.AddSingleton<IInboxWorkerTransport, KafkaInboxWorkerTransport>();

            services.AddSingleton<ITransportTopicWithWildCard, KafkaTransportTopicWithWildCard>();
        }
    }
}
