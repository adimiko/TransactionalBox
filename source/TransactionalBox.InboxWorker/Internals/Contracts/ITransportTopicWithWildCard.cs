namespace TransactionalBox.InboxWorker.Internals.Contracts
{
    internal interface ITransportTopicWithWildCard
    {
        public string GetTopicWithWildCard(string serviceName);
    }
}
