using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Transport.Kafka.Internals;
using TransactionalBox.Inbox.Transport.Kafka.Settings;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals.Transport;

namespace TransactionalBox.Inbox.Transport.Kafka
{
    public static class Extensions
    {
        public static void UseKafka(
            this IInboxTransportConfigurator inboxWorkerTransportConfigurator,
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
