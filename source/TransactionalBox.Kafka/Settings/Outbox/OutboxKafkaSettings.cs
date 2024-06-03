using TransactionalBox.Kafka.Internals.Outbox;

namespace TransactionalBox.Kafka.Settings.Outbox
{
    public sealed class OutboxKafkaSettings : IOutboxKafkaSettings
    {
        public string BootstrapServers { get; set; }

        public KafkaTransportMessageSizeSettings TransportMessageSizeSettings { get; } = new KafkaTransportMessageSizeSettings();

        internal OutboxKafkaSettings() { }
    }
}
