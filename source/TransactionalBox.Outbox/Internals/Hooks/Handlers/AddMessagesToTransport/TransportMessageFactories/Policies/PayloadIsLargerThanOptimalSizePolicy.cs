using System.Text.Json;
using System.Text;
using TransactionalBox.Outbox.Internals.Compression;
using TransactionalBox.Outbox.Internals.Storage;
using TransactionalBox.Outbox.Internals.Transport;

namespace TransactionalBox.Outbox.Internals.Hooks.Handlers.AddMessagesToTransport.TransportMessageFactories.Policies
{
    internal sealed class PayloadIsLargerThanOptimalSizePolicy : IPayloadCreationPolicy
    {
        private readonly ICompressionAlgorithm _compressionAlgorithm;

        private readonly ITransportMessageSizeSettings _settings;

        public PayloadIsLargerThanOptimalSizePolicy(
            ICompressionAlgorithm compressionAlgorithm,
            ITransportMessageSizeSettings settings)
        {
            _compressionAlgorithm = compressionAlgorithm;
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

                var cpayload = await _compressionAlgorithm.Compress(payload);

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
