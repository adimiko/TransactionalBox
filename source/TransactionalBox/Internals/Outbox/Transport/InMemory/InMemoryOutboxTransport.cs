using TransactionalBox.Internals.Outbox.Hooks.Handlers.AddMessagesToTransport.TransportMessageFactories;
using TransactionalBox.Internals.Outbox.Transport.ContractsToImplement;
using TransactionalBox.Internals.Transport.InMemory.Internals;

namespace TransactionalBox.Internals.Outbox.Transport.InMemory
{
    internal sealed class InMemoryOutboxTransport : IOutboxTransport
    {
        private readonly IInMemoryTransport _inMemoryTransport;

        public InMemoryOutboxTransport(IInMemoryTransport inMemoryTransport)
        {
            _inMemoryTransport = inMemoryTransport;
        }

        public async Task Add(TransportEnvelope transportEnvelope)
        {
            var transportObject = new TransportObject()
            {
                Topic = transportEnvelope.Topic,
                Payload = transportEnvelope.Payload,
                Compression = transportEnvelope.Compression,
            };

            await _inMemoryTransport.Writer.WriteAsync(transportObject);
        }
    }
}
