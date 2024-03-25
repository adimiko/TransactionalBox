using TransactionalBox.InboxBase.StorageModel.Internals;

namespace TransactionalBox.InboxWorker.Internals.Contracts
{
    internal interface IInboxWorkerTransport
    {
        IAsyncEnumerable<byte[]> GetMessages(CancellationToken cancellationToken);
    }
}
