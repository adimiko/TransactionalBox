using TransactionalBox.InboxWorker.Internals.Contracts;

namespace TransactionalBox.Base.Transport.InMemory.Internals
{
    internal sealed class InMemoryTransportTopicWithWildCard : ITransportTopicWithWildCard
    {
        public string GetTopicWithWildCard(string serviceName) => serviceName + '*';
    }
}
