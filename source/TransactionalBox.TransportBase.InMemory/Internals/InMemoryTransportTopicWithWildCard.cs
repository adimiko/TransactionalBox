using TransactionalBox.InboxWorker.Internals.Contracts;

namespace TransactionalBox.TransportBase.InMemory.Internals
{
    internal sealed class InMemoryTransportTopicWithWildCard : ITransportTopicWithWildCard
    {
        public string GetTopicWithWildCard(string serviceName) => serviceName + '*';
    }
}
