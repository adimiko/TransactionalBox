namespace TransactionalBox.Outbox.Transport.Kafka.Internals
{
    internal interface IOutboxWorkerKafkaSettings
    {
        string BootstrapServers { get; }
    }
}
