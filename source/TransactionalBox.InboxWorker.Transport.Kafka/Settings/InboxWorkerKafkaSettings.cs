using TransactionalBox.InboxWorker.Transport.Kafka.Internals;

namespace TransactionalBox.InboxWorker.Transport.Kafka.Settings
{
    public sealed class InboxWorkerKafkaSettings : IInboxWorkerKafkaSettings
    {
        public string BootstrapServers { get; set; }

        internal InboxWorkerKafkaSettings() { }
    }
}
