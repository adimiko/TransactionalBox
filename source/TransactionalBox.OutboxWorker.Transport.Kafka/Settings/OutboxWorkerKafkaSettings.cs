using TransactionalBox.OutboxWorker.Internals.Contracts;
using TransactionalBox.OutboxWorker.Transport.Kafka.Internals;

namespace TransactionalBox.OutboxWorker.Transport.Kafka.Settings
{
    public sealed class OutboxWorkerKafkaSettings : IOutboxWorkerKafkaSettings
    {
        public string BootstrapServers { get; set; }

        public KafkaTransportMessageSizeSettings TransportMessageSizeSettings { get; } = new KafkaTransportMessageSizeSettings();

        internal OutboxWorkerKafkaSettings() { }
    }
}
