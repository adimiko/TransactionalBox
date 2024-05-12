using System.Text;
using System.Text.Json;
using TransactionalBox.Outbox.Internals.Compression;
using TransactionalBox.Outbox.Internals.Hooks.Handlers.AddMessagesToTransport.TransportMessageFactories.Policies;
using TransactionalBox.Outbox.Internals.Storage;
using TransactionalBox.Outbox.Internals.Transport;

namespace TransactionalBox.Outbox.Internals.Hooks.Handlers.AddMessagesToTransport.TransportMessageFactories
{
    internal sealed class TransportMessageFactory
    {
        private readonly ICompression _compression;

        private readonly IEnumerable<IPayloadCreationPolicy> _payloadCreationPolicies;

        private readonly ITransportMessageSizeSettings _transportMessageSizeSettings;

        public TransportMessageFactory(
            ICompression compression,
            IEnumerable<IPayloadCreationPolicy> payloadCreationPolicies,
            ITransportMessageSizeSettings transportMessageSizeSettings)
        {
            _compression = compression;
            _payloadCreationPolicies = payloadCreationPolicies;
            _transportMessageSizeSettings = transportMessageSizeSettings;
        }

        public async Task<IEnumerable<TransportMessage>> Create(IEnumerable<OutboxMessageStorage> outboxMessages)
        {
            var groupedOutboxMessagesByTopic = outboxMessages
            .GroupBy(x => x.Topic)
            .Select(groupedMessagesWithTheSameTopic => new GroupedOutboxMessagesWithTheSameTopic()
            {
                Topic = groupedMessagesWithTheSameTopic.Key,
                Messages = groupedMessagesWithTheSameTopic
            });

            var transportMessages = new List<TransportMessage>();

            foreach (var groupedOutboxMessagesWithTheSameTopic in groupedOutboxMessagesByTopic)
            {
                var messages = groupedOutboxMessagesWithTheSameTopic.Messages;

                var payload = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(messages)); //TODO TransportSerializer with StringBuilder

                var compressedPayload = await _compression.Compress(payload);

                var compressedPayloadSize = compressedPayload.Length;

                var policy = _payloadCreationPolicies.First(x => x.IsApplicable(compressedPayloadSize));

                var payloads = await policy.Execute(compressedPayload, messages);

                foreach (var p in payloads)
                {
                    var transportMessage = new TransportMessage()
                    {
                        Topic = groupedOutboxMessagesWithTheSameTopic.Topic,
                        Payload = p,
                    };

                    transportMessages.Add(transportMessage);
                }
            }

            return transportMessages;
        }
    }
}
