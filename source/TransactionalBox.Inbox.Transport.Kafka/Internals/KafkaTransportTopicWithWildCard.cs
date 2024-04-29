using TransactionalBox.Inbox.Internals.Contracts;

namespace TransactionalBox.Inbox.Transport.Kafka.Internals
{
    internal sealed class KafkaTransportTopicWithWildCard : ITransportTopicWithWildCard
    {
        public string GetTopicWithWildCard(string serviceName) => $"^{serviceName}.*";
    }
}
