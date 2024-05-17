using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals.Transport.Topics;
using TransactionalBox.Inbox.Internals.Transport;
using TransactionalBox.Inbox.Kafka.Internals;
using TransactionalBox.Inbox.Kafka.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Inbox
{
    public static class ExtensionUseKafka
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
            services.AddSingleton<ITransportTopicsCreator, KafkaTransportTopicsCreator>();
        }
    }
}
