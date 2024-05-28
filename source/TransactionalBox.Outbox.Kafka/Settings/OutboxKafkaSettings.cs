using TransactionalBox.Outbox.Kafka.Internals;

namespace TransactionalBox.Outbox.Kafka.Settings
{
    public sealed class OutboxKafkaSettings : IOutboxKafkaSettings
    {
        public string BootstrapServers { get; set; }

        internal OutboxKafkaSettings() { }
    }
}
