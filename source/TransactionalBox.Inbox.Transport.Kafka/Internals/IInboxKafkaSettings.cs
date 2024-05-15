namespace TransactionalBox.Inbox.Transport.Kafka.Internals
{
    internal interface IInboxKafkaSettings
    {
        string BootstrapServers { get; }
    }
}
