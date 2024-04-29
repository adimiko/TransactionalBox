namespace TransactionalBox.Inbox.Transport.Kafka.Internals
{
    internal interface IInboxWorkerKafkaSettings
    {
        string BootstrapServers { get; }
    }
}
