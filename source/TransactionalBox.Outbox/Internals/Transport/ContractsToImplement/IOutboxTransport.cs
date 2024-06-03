using TransactionalBox.Outbox.Internals.Hooks.Handlers.AddMessagesToTransport.TransportMessageFactories;

namespace TransactionalBox.Outbox.Internals.Transport.ContractsToImplement
{
    internal interface IOutboxTransport
    {
        Task Add(TransportEnvelope transportEnvelope);
    }
}
