using TransactionalBox.InboxBase.StorageModel.Internals;

namespace TransactionalBox.InboxWorker.Internals.Contracts
{
    internal interface IInboxWorkerTransport
    {
        IAsyncEnumerable<IEnumerable<InboxMessage>> GetMessages(CancellationToken cancellationToken);
    }
}
