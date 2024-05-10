using TransactionalBox.Outbox.Internals.Storage;
using TransactionalBox.Outbox.Internals.Transport;

namespace TransactionalBox.Outbox.Internals.Hooks.AddMessagesToTransport.TransportMessageFactories.Policies
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
