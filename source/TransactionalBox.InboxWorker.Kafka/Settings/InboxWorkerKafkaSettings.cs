using TransactionalBox.InboxWorker.Kafka.Internals;

namespace TransactionalBox.InboxWorker.Kafka.Settings
{
    public sealed class InboxWorkerKafkaSettings : IInboxWorkerKafkaSettings
    {
        public string BootstrapServers { get; set; }

        internal InboxWorkerKafkaSettings() { }
    }
}
