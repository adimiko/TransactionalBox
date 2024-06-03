using TransactionalBox.Internals.Outbox.Storage;
using TransactionalBox.Internals.Outbox.Transport.ContractsToImplement;

namespace TransactionalBox.Internals.Outbox.Hooks.Handlers.AddMessagesToTransport.TransportMessageFactories.Policies
{
    internal sealed class PayloadHasOptimalSizePolicy : IPayloadCreationPolicy
    {
        private readonly ITransportMessageSizeSettings _settings;

        public PayloadHasOptimalSizePolicy(ITransportMessageSizeSettings settings)
        {
            _settings = settings;
        }

        public Task<IEnumerable<byte[]>> Execute(byte[] compressedPayload, IEnumerable<OutboxMessageStorage> outboxMessages)
        {
            return Task.FromResult<IEnumerable<byte[]>>(new List<byte[]> { compressedPayload });
        }

        public bool IsApplicable(int compressedPayloadSize)
        {
            return compressedPayloadSize <= _settings.OptimalTransportMessageSize;
        }
    }
}
