using TransactionalBox.Internals.Outbox.Storage;

namespace TransactionalBox.Internals.Outbox.Hooks.Handlers.AddMessagesToTransport.TransportMessageFactories.Policies
{
    internal interface IPayloadCreationPolicy
    {
        bool IsApplicable(int compressedPayloadSize);

        Task<IEnumerable<byte[]>> Execute(byte[] compressedPayload, IEnumerable<OutboxMessageStorage> outboxMessages);
    }
}
