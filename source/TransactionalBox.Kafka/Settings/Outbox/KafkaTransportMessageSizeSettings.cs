using TransactionalBox.Internals.Outbox.Transport.ContractsToImplement;

namespace TransactionalBox.Kafka.Settings.Outbox
{
    public sealed class KafkaTransportMessageSizeSettings : ITransportMessageSizeSettings
    {
        public int OptimalTransportMessageSize { get; set; } = 10240;

        internal KafkaTransportMessageSizeSettings() { }
    }
}
