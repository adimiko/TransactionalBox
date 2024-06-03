using TransactionalBox.Outbox.Internals.Transport.ContractsToImplement;

namespace TransactionalBox.Outbox.Kafka.Settings
{
    public sealed class KafkaTransportMessageSizeSettings : ITransportMessageSizeSettings
    {
        public int OptimalTransportMessageSize { get; set; } = 10240;

        internal KafkaTransportMessageSizeSettings() { }
    }
}
