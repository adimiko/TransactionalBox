using TransactionalBox.OutboxWorker.Transport.Kafka.Internals;

namespace TransactionalBox.OutboxWorker.Transport.Kafka.Settings
{
    public sealed class OutboxWorkerKafkaSettings : IOutboxWorkerKafkaSettings
    {
        public string BootstrapServers { get; set; }

        internal OutboxWorkerKafkaSettings() { }
    }
}
