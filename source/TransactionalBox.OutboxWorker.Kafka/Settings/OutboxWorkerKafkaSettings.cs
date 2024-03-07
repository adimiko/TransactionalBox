namespace TransactionalBox.OutboxWorker.Kafka.Settings
{
    public sealed class OutboxWorkerKafkaSettings
    {
        public string BootstrapServers { get; set; }

        internal OutboxWorkerKafkaSettings() { }
    }
}
