namespace TransactionalBox.OutboxWorker.Transport.Kafka.Internals
{
    internal interface IOutboxWorkerKafkaSettings
    {
        string BootstrapServers { get; }
    }
}
