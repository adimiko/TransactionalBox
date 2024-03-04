namespace TransactionalBox.InboxWorker.Internals
{
    public interface IInboxWorkerTransport
    {
        IAsyncEnumerable<string> GetMessage(CancellationToken cancellationToken);
    }
}
