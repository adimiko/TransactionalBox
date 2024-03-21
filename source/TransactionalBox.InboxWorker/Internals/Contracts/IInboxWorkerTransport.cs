using TransactionalBox.InboxBase.StorageModel;

namespace TransactionalBox.InboxWorker.Internals.Contracts
{
    internal interface IInboxWorkerTransport
    {
        IAsyncEnumerable<IEnumerable<InboxMessage>> GetMessages(CancellationToken cancellationToken);
    }
}
