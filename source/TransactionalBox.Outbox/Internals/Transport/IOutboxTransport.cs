using TransactionalBox.Outbox.Internals.Hooks.Handlers.AddMessagesToTransport.TransportMessageFactories;

namespace TransactionalBox.Outbox.Internals.Transport
{
    internal interface IOutboxTransport
    {
        Task Add(TransportEnvelope transportEnvelope);
    }
}
