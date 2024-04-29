namespace TransactionalBox.Inbox.Internals.Contracts
{
    internal interface IInboxWorkerTransport
    {
        IAsyncEnumerable<byte[]> GetMessages(IEnumerable<string> topics, CancellationToken cancellationToken);
    }
}
