using TransactionalBox.Inbox.Transport.Kafka.Internals;

namespace TransactionalBox.Inbox.Transport.Kafka.Settings
{
    public sealed class InboxWorkerKafkaSettings : IInboxWorkerKafkaSettings
    {
        public string BootstrapServers { get; set; }

        internal InboxWorkerKafkaSettings() { }
    }
}
