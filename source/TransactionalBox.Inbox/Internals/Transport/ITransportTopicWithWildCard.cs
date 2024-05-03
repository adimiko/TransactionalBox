namespace TransactionalBox.Inbox.Internals.Transport
{
    internal interface ITransportTopicWithWildCard
    {
        public string GetTopicWithWildCard(string serviceName);
    }
}
