using TransactionalBox.Base.Transport.InMemory.Internals;

namespace TransactionalBox.Outbox.Internals.Transport.InMemory
{
    internal sealed class InMemoryOutboxWorkerTransport : IOutboxWorkerTransport
    {
        private readonly IInMemoryTransport _inMemoryTransport;

        public InMemoryOutboxWorkerTransport(IInMemoryTransport inMemoryTransport) 
        {
            _inMemoryTransport = inMemoryTransport;
        }

        public async Task<TransportResult> Add(string topic, byte[] payload)
        {
            try
            {
                var transportObject = new TransportObject()
                {
                    Topic = topic,
                    Payload = payload
                };

                await _inMemoryTransport .Writer.WriteAsync(transportObject);

                return TransportResult.Success;
            }
            catch
            {
                return TransportResult.Failure;
            }
        }
    }
}
