using TransactionalBox.InboxBase.StorageModel;

namespace TransactionalBox.InboxWorker.Internals
{
    public interface IInboxWorkerTransport
    {
        IAsyncEnumerable<IEnumerable<InboxMessage>> GetMessages(CancellationToken cancellationToken);
    }
}
