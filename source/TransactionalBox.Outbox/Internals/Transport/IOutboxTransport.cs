namespace TransactionalBox.Outbox.Internals.Transport
{
    internal interface IOutboxTransport
    {
        Task Add(string topic, byte[] payload);
    }
}
