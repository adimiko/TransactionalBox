namespace TransactionalBox.InboxWorker.Transport.Kafka.Internals
{
    internal interface IInboxWorkerKafkaSettings
    {
        string BootstrapServers { get; }
    }
}
