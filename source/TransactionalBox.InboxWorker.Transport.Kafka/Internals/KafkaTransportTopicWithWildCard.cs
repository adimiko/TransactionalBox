using TransactionalBox.InboxWorker.Internals.Contracts;

namespace TransactionalBox.InboxWorker.Transport.Kafka.Internals
{
    internal sealed class KafkaTransportTopicWithWildCard : ITransportTopicWithWildCard
    {
        public string GetTopicWithWildCard(string serviceName) => $"^{serviceName}.*";
    }
}
