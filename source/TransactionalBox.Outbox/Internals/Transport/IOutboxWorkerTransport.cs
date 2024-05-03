namespace TransactionalBox.Outbox.Internals.Transport
{
    internal interface IOutboxWorkerTransport
    {
        Task<TransportResult> Add(string topic, byte[] payload);
    }
}
