using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Transport.Kafka.Internals;
using TransactionalBox.Inbox.Transport.Kafka.Settings;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals.Transport;
using TransactionalBox.Inbox.Internals.Transport.Topics;

namespace TransactionalBox.Inbox.Transport.Kafka
{
    public static class Extensions
    {
        public static void UseKafka(
            this IInboxTransportConfigurator inboxWorkerTransportConfigurator,
            Action<InboxKafkaSettings> settingsConfiguration = null)
        {
            var services = inboxWorkerTransportConfigurator.Services;
            var settings = new InboxKafkaSettings();

            if (settingsConfiguration is not null) 
            {
                settingsConfiguration(settings);
            }

            services.AddSingleton<IInboxKafkaSettings>(settings);
            services.AddSingleton<KafkaConsumerConfigFactory>();
            services.AddSingleton<IInboxTransport, KafkaInboxTransport>();
            services.AddSingleton<ITransportTopicsCreator,  KafkaTransportTopicsCreator>();
        }
    }
}
