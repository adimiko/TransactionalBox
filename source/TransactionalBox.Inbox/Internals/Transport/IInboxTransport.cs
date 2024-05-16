namespace TransactionalBox.Inbox.Internals.Transport
{
    internal interface IInboxTransport
    {
        IAsyncEnumerable<byte[]> GetMessages(IEnumerable<string> topics, CancellationToken cancellationToken);
    }
}
