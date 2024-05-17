namespace TransactionalBox.Outbox.Kafka.Internals
{
    internal interface IOutboxKafkaSettings
    {
        string BootstrapServers { get; }
    }
}
