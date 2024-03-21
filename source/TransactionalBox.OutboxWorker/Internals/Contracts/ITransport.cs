using TransactionalBox.OutboxBase.StorageModel;

namespace TransactionalBox.OutboxWorker.Internals.Contracts
{
    internal interface ITransport
    {
        Task<TransportResult> Add(IEnumerable<TransportMessage> transportMessages);
    }
}
