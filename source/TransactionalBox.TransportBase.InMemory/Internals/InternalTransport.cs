﻿using System.Threading.Channels;
using TransactionalBox.InboxWorker.Internals.Contracts;
using TransactionalBox.OutboxWorker.Internals.Contracts;

namespace TransactionalBox.TransportBase.InMemory.Internals
{
    internal sealed class InternalTransport : IOutboxWorkerTransport, IInboxWorkerTransport
    {
        private static readonly Channel<TransportObject> _channel = Channel.CreateUnbounded<TransportObject>();

        private ChannelWriter<TransportObject> _writer = _channel.Writer;

        private ChannelReader<TransportObject> _reader = _channel.Reader;

        public async Task<TransportResult> Add(string topic, byte[] payload)
        {
            try
            {
                var transportObject = new TransportObject()
                {
                    Topic = topic,
                    Payload = payload
                };

                await _writer.WriteAsync(transportObject);

                return TransportResult.Success;
            }
            catch
            {
                return TransportResult.Failure;
            }
        }

        public async IAsyncEnumerable<byte[]> GetMessages(IEnumerable<string> topics, CancellationToken cancellationToken)
        {
            await foreach (var message in _reader.ReadAllAsync(cancellationToken)) 
            {
                //TODO should return topics with messages ?

                yield return message.Payload;
            }
        }
    }
}
