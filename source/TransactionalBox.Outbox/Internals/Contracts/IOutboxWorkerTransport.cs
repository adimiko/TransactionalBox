namespace TransactionalBox.Outbox.Internals.Contracts
{
    internal interface IOutboxWorkerTransport
    {
        Task<TransportResult> Add(string topic, byte[] payload);
    }
}
