using TransactionalBox.OutboxBase.StorageModel;

namespace TransactionalBox.OutboxWorker.Internals.Contracts
{
    public interface ITransport
    {
        Task<TransportResult> Add(IEnumerable<TransportMessage> transportMessages);
    }
}
