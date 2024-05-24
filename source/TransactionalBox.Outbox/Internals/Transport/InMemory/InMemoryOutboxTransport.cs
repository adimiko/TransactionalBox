﻿using TransactionalBox.Internals.Transport.InMemory.Internals;

namespace TransactionalBox.Outbox.Internals.Transport.InMemory
{
    internal sealed class InMemoryOutboxTransport : IOutboxTransport
    {
        private readonly IInMemoryTransport _inMemoryTransport;

        public InMemoryOutboxTransport(IInMemoryTransport inMemoryTransport)
        {
            _inMemoryTransport = inMemoryTransport;
        }

        public async Task Add(string topic, byte[] payload, string contentType)
        {
            var transportObject = new TransportObject()
            {
                Topic = topic,
                Payload = payload,
                Compression = contentType
            };

            await _inMemoryTransport.Writer.WriteAsync(transportObject);
        }
    }
}
