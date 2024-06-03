using System.Text;
using System.Text.Json;
using TransactionalBox.Outbox.Internals.Compression;
using TransactionalBox.Outbox.Internals.Hooks.Handlers.AddMessagesToTransport.TransportMessageFactories.Policies;
using TransactionalBox.Outbox.Internals.Storage;
using TransactionalBox.Outbox.Internals.Transport.ContractsToImplement;

namespace TransactionalBox.Outbox.Internals.Hooks.Handlers.AddMessagesToTransport.TransportMessageFactories
{
    internal sealed class TransportEnvelopeFactory
    {
        private readonly ICompression _compression;

        private readonly IEnumerable<IPayloadCreationPolicy> _payloadCreationPolicies;

        private readonly ITransportMessageSizeSettings _transportMessageSizeSettings;

        public TransportEnvelopeFactory(
            ICompression compression,
            IEnumerable<IPayloadCreationPolicy> payloadCreationPolicies,
            ITransportMessageSizeSettings transportMessageSizeSettings)
        {
            _compression = compression;
            _payloadCreationPolicies = payloadCreationPolicies;
            _transportMessageSizeSettings = transportMessageSizeSettings;
        }

        public async Task<IEnumerable<TransportEnvelope>> Create(IEnumerable<OutboxMessageStorage> outboxMessages)
        {
            var groupedOutboxMessagesByTopic = outboxMessages
            .GroupBy(x => x.Topic)
            .Select(groupedMessagesWithTheSameTopic => new GroupedOutboxMessagesWithTheSameTopic()
            {
                Topic = groupedMessagesWithTheSameTopic.Key,
                Messages = groupedMessagesWithTheSameTopic
            });

            var transportEnvelopes = new List<TransportEnvelope>();

            foreach (var groupedOutboxMessagesWithTheSameTopic in groupedOutboxMessagesByTopic)
            {
                var messages = groupedOutboxMessagesWithTheSameTopic.Messages;

                var transportMessages = new List<TransportMessage>();

                foreach (var message in messages)
                {
                    transportMessages.Add(new TransportMessage(message.Id, message.Topic, message.OccurredUtc, message.Payload));
                }    


                var payload = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(transportMessages)); //TODO TransportSerializer with StringBuilder

                var compressedPayload = await _compression.Compress(payload);

                var compressedPayloadSize = compressedPayload.Length;

                var policy = _payloadCreationPolicies.First(x => x.IsApplicable(compressedPayloadSize));

                var payloads = await policy.Execute(compressedPayload, messages);

                foreach (var p in payloads)
                {
                    var transportEnvelope= new TransportEnvelope()
                    {
                        Topic = groupedOutboxMessagesWithTheSameTopic.Topic,
                        Payload = p,
                        Compression = _compression.Name,
                    };

                    transportEnvelopes.Add(transportEnvelope);
                }
            }

            return transportEnvelopes;
        }
    }
}
