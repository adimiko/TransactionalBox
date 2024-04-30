using TransactionalBox.Outbox.Transport.Kafka.Internals;

namespace TransactionalBox.Outbox.Transport.Kafka.Settings
{
    public sealed class OutboxWorkerKafkaSettings : IOutboxWorkerKafkaSettings
    {
        public string BootstrapServers { get; set; }

        public KafkaTransportMessageSizeSettings TransportMessageSizeSettings { get; } = new KafkaTransportMessageSizeSettings();

        internal OutboxWorkerKafkaSettings() { }
    }
}
