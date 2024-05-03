using TransactionalBox.Inbox.Internals.Transport;

namespace TransactionalBox.Inbox.Transport.Kafka.Internals
{
    internal sealed class KafkaTransportTopicWithWildCard : ITransportTopicWithWildCard
    {
        public string GetTopicWithWildCard(string serviceName) => $"^{serviceName}.*";
    }
}
