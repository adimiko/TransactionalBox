namespace TransactionalBox.Inbox.Kafka.Internals
{
    internal interface IInboxKafkaSettings
    {
        string BootstrapServers { get; }
    }
}
