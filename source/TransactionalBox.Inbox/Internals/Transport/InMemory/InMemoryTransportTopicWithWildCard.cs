namespace TransactionalBox.Inbox.Internals.Transport.InMemory
{
    internal sealed class InMemoryTransportTopicWithWildCard : ITransportTopicWithWildCard
    {
        public string GetTopicWithWildCard(string serviceName) => serviceName + '*';
    }
}
