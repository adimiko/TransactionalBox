using TransactionalBox.Inbox.Transport.Kafka.Internals;

namespace TransactionalBox.Inbox.Transport.Kafka.Settings
{
    public sealed class InboxKafkaSettings : IInboxKafkaSettings
    {
        public string BootstrapServers { get; set; }

        internal InboxKafkaSettings() { }
    }
}
