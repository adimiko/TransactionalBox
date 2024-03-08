namespace TransactionalBox.OutboxWorker.Kafka.Internals
{
    internal interface IOutboxWorkerKafkaSettings
    {
        string BootstrapServers { get; }
    }
}
