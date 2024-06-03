namespace TransactionalBox.Kafka.Internals.Inbox
{
    internal interface IInboxKafkaSettings
    {
        string BootstrapServers { get; }
    }
}
