using TransactionalBox.Outbox.Internals.Storage;

namespace TransactionalBox.Outbox.Internals.Hooks.Handlers.AddMessagesToTransport.TransportMessageFactories.Policies
{
    internal interface IPayloadCreationPolicy
    {
        bool IsApplicable(int compressedPayloadSize);

        Task<IEnumerable<byte[]>> Execute(byte[] compressedPayload, IEnumerable<OutboxMessageStorage> outboxMessages);
    }
}
