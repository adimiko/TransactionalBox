namespace TransactionalBox.Outbox.Internals.Transport
{
    internal interface IOutboxWorkerTransport
    {
        Task Add(string topic, byte[] payload);
    }
}
