namespace TransactionalBox.Inbox.Internals.Transport
{
    internal interface IInboxTransport
    {
        IAsyncEnumerable<TransportMessage> GetMessages(IEnumerable<string> topics, CancellationToken cancellationToken);
    }
}
