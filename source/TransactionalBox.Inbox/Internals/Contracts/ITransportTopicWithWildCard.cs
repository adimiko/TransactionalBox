namespace TransactionalBox.Inbox.Internals.Contracts
{
    internal interface ITransportTopicWithWildCard
    {
        public string GetTopicWithWildCard(string serviceName);
    }
}
