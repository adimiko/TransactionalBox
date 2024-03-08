using TransactionalBox.InboxBase.StorageModel;

namespace TransactionalBox.InboxWorker.Internals
{
    public interface IInboxWorkerTransport
    {
        IAsyncEnumerable<InboxMessage> GetMessage(CancellationToken cancellationToken);
    }
}
