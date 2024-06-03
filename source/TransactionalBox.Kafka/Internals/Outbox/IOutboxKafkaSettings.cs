namespace TransactionalBox.Kafka.Internals.Outbox
{
    internal interface IOutboxKafkaSettings
    {
        string BootstrapServers { get; }
    }
}
