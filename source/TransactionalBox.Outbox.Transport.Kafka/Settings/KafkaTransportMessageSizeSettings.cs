using TransactionalBox.Outbox.Internals.Transport;

namespace TransactionalBox.Outbox.Transport.Kafka.Settings
{
    public sealed class KafkaTransportMessageSizeSettings : ITransportMessageSizeSettings
    {
        public int OptimalTransportMessageSize { get; set; } = 10240;

        internal KafkaTransportMessageSizeSettings() { }
    }
}
