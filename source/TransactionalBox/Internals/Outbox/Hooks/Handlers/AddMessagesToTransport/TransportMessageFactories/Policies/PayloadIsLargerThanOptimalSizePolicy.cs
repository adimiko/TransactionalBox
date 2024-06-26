﻿using System.Text.Json;
using System.Text;
using TransactionalBox.Internals.Outbox.Storage;
using TransactionalBox.Internals.Outbox.Transport.ContractsToImplement;
using TransactionalBox.Internals.Outbox.Compression;

namespace TransactionalBox.Internals.Outbox.Hooks.Handlers.AddMessagesToTransport.TransportMessageFactories.Policies
{
    internal sealed class PayloadIsLargerThanOptimalSizePolicy : IPayloadCreationPolicy
    {
        private readonly ICompression _compression;

        private readonly ITransportMessageSizeSettings _settings;

        public PayloadIsLargerThanOptimalSizePolicy(
            ICompression compression,
            ITransportMessageSizeSettings settings)
        {
            _compression = compression;
            _settings = settings;
        }

        public async Task<IEnumerable<byte[]>> Execute(byte[] compressedPayload, IEnumerable<OutboxMessageStorage> outboxMessages)
        {
            var numberOfMessages = outboxMessages.Count();

            if (numberOfMessages <= 1)
            {
                // (Improvment) when message is larger than optimal size can be splitted to small messages (not this stage of the project)
                return new List<byte[]> { compressedPayload };
            }

            var compressedPayloadSize = compressedPayload.Length;

            var splitRatio = Math.Ceiling(Convert.ToDecimal(compressedPayloadSize) / _settings.OptimalTransportMessageSize);

            var chunkSize = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(numberOfMessages) / splitRatio));

            var chunks = outboxMessages.Chunk(chunkSize);

            var payloads = new List<byte[]>();

            foreach (var chunk in chunks)
            {
                var payload = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(chunk)); //TODO TransportSerializer with StringBuilder

                var cpayload = await _compression.Compress(payload);

                payloads.Add(cpayload);
            }

            return payloads;
        }

        public bool IsApplicable(int compressedPayloadSize)
        {
            return compressedPayloadSize > _settings.OptimalTransportMessageSize;
        }
    }
}
