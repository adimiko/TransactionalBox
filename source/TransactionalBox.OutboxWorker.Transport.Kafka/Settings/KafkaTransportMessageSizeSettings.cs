using TransactionalBox.OutboxWorker.Internals.Contracts;

namespace TransactionalBox.OutboxWorker.Transport.Kafka.Settings
{
    public sealed class KafkaTransportMessageSizeSettings : ITransportMessageSizeSettings
    {
        public int OptimalTransportMessageSize { get; set; } = 10240;

        internal KafkaTransportMessageSizeSettings() { }
    }
}
