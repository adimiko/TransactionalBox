namespace TransactionalBox.InboxWorker.Kafka.Internals
{
    internal interface IInboxWorkerKafkaSettings
    {
        string BootstrapServers { get; }
    }
}
