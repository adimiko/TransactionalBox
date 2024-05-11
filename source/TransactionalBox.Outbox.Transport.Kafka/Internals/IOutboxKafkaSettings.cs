namespace TransactionalBox.Outbox.Transport.Kafka.Internals
{
    internal interface IOutboxKafkaSettings
    {
        string BootstrapServers { get; }
    }
}
