using TransactionalBox.Base.Inbox.StorageModel.Internals;

namespace TransactionalBox.InboxWorker.Internals.Contracts
{
    internal interface IInboxWorkerTransport
    {
        IAsyncEnumerable<byte[]> GetMessages(IEnumerable<string> topics, CancellationToken cancellationToken);
    }
}
