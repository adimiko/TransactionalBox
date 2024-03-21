using TransactionalBox.InboxBase.StorageModel;

namespace TransactionalBox.InboxWorker.Internals.Contracts
{
    public interface IInboxWorkerTransport
    {
        IAsyncEnumerable<IEnumerable<InboxMessage>> GetMessages(CancellationToken cancellationToken);
    }
}
