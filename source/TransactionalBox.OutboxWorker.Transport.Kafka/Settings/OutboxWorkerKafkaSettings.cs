using TransactionalBox.OutboxWorker.Internals.Contracts;
using TransactionalBox.OutboxWorker.Transport.Kafka.Internals;

namespace TransactionalBox.OutboxWorker.Transport.Kafka.Settings
{
    public sealed class OutboxWorkerKafkaSettings : IOutboxWorkerKafkaSettings, ITransportMessageSizeSettings
    {
        public string BootstrapServers { get; set; }

        public int OptimalTransportMessageSize { get; set; } = 10240;

        internal OutboxWorkerKafkaSettings() { }
    }
}
