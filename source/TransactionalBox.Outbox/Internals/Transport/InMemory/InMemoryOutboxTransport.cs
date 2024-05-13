using TransactionalBox.Base.Transport.InMemory.Internals;

namespace TransactionalBox.Outbox.Internals.Transport.InMemory
{
    internal sealed class InMemoryOutboxTransport : IOutboxTransport
    {
        private readonly IInMemoryTransport _inMemoryTransport;

        public InMemoryOutboxTransport(IInMemoryTransport inMemoryTransport)
        {
            _inMemoryTransport = inMemoryTransport;
        }

        public async Task Add(string topic, byte[] payload)
        {
            var transportObject = new TransportObject()
            {
                Topic = topic,
                Payload = payload
            };

            await _inMemoryTransport.Writer.WriteAsync(transportObject);
        }
    }
}
