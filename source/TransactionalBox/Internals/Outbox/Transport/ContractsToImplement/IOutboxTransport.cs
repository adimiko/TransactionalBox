using TransactionalBox.Internals.Outbox.Hooks.Handlers.AddMessagesToTransport.TransportMessageFactories;
namespace TransactionalBox.Internals.Outbox.Transport.ContractsToImplement
{
    internal interface IOutboxTransport
    {
        Task Add(TransportEnvelope transportEnvelope);
    }
}
