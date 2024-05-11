using TransactionalBox.Outbox.Transport.Kafka.Internals;

namespace TransactionalBox.Outbox.Transport.Kafka.Settings
{
    public sealed class OutboxKafkaSettings : IOutboxKafkaSettings
    {
        public string BootstrapServers { get; set; }

        public KafkaTransportMessageSizeSettings TransportMessageSizeSettings { get; } = new KafkaTransportMessageSizeSettings();

        internal OutboxKafkaSettings() { }
    }
}
