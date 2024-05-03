namespace TransactionalBox.Inbox.Internals.Transport
{
    internal interface IInboxWorkerTransport
    {
        IAsyncEnumerable<byte[]> GetMessages(IEnumerable<string> topics, CancellationToken cancellationToken);
    }
}
