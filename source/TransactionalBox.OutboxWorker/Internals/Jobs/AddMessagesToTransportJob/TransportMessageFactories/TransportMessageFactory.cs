using System.Text;
using System.Text.Json;
using TransactionalBox.Base.Outbox.StorageModel.Internals;
using TransactionalBox.OutboxWorker.Compression;
using TransactionalBox.OutboxWorker.Internals.Contracts;
using TransactionalBox.OutboxWorker.Internals.Jobs.AddMessagesToTransportJob.TransportMessageFactories.Policies;

namespace TransactionalBox.OutboxWorker.Internals.Jobs.AddMessagesToTransportJob.TransportMessageFactories
{
    internal sealed class TransportMessageFactory
    {
        private readonly ICompressionAlgorithm _compressionAlgorithm;

        private readonly IEnumerable<IPayloadCreationPolicy> _payloadCreationPolicies;

        private readonly ITransportMessageSizeSettings _transportMessageSizeSettings;

        public TransportMessageFactory(
            ICompressionAlgorithm compressionAlgorithm,
            IEnumerable<IPayloadCreationPolicy> payloadCreationPolicies,
            ITransportMessageSizeSettings transportMessageSizeSettings)
        {
            _compressionAlgorithm = compressionAlgorithm;
            _payloadCreationPolicies = payloadCreationPolicies;
            _transportMessageSizeSettings = transportMessageSizeSettings;
        }

        public async Task<IEnumerable<TransportMessage>> Create(IEnumerable<OutboxMessage> outboxMessages)
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

                var compressedPayload = await _compressionAlgorithm.Compress(payload);

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
