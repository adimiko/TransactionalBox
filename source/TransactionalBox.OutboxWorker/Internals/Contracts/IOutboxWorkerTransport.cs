using TransactionalBox.Base.Outbox.StorageModel;

namespace TransactionalBox.OutboxWorker.Internals.Contracts
{
    internal interface IOutboxWorkerTransport
    {
        Task<TransportResult> Add(string topic, byte[] payload);
    }
}
