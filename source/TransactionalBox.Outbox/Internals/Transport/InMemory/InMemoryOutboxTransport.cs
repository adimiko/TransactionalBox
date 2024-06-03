using TransactionalBox.Internals.Transport.InMemory.Internals;
using TransactionalBox.Outbox.Internals.Hooks.Handlers.AddMessagesToTransport.TransportMessageFactories;
using TransactionalBox.Outbox.Internals.Transport.ContractsToImplement;

namespace TransactionalBox.Outbox.Internals.Transport.InMemory
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
