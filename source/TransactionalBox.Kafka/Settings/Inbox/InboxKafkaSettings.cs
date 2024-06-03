using TransactionalBox.Kafka.Internals.Inbox;

namespace TransactionalBox.Kafka.Settings.Inbox
{
    public sealed class InboxKafkaSettings : IInboxKafkaSettings
    {
        public string BootstrapServers { get; set; }

        internal InboxKafkaSettings() { }
    }
}
