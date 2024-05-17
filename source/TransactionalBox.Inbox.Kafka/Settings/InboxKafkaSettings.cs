using TransactionalBox.Inbox.Kafka.Internals;

namespace TransactionalBox.Inbox.Kafka.Settings
{
    public sealed class InboxKafkaSettings : IInboxKafkaSettings
    {
        public string BootstrapServers { get; set; }

        internal InboxKafkaSettings() { }
    }
}
