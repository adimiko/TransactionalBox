using TransactionalBox.OutboxWorker.Kafka.Internals;

namespace TransactionalBox.OutboxWorker.Kafka.Settings
{
    public sealed class OutboxWorkerKafkaSettings : IOutboxWorkerKafkaSettings
    {
        public string BootstrapServers { get; set; }

        internal OutboxWorkerKafkaSettings() { }
    }
}
