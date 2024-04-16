using System.Text;
using System.Text.Json;
using TransactionalBox.Base.Outbox.StorageModel.Internals;
using TransactionalBox.OutboxWorker.Compression;
using TransactionalBox.OutboxWorker.Internals.Contracts;

namespace TransactionalBox.OutboxWorker.Internals
{
    internal sealed class TransportMessageFactory
    {
        private readonly ICompressionAlgorithm _compressionAlgorithm;

        private readonly ITransportMessageSizeSettings _transportMessageSizeSettings;

        public TransportMessageFactory(
            ICompressionAlgorithm compressionAlgorithm,
            ITransportMessageSizeSettings transportMessageSizeSettings) 
        {
            _compressionAlgorithm = compressionAlgorithm;
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

                var compressedPayloadBytes = compressedPayload.Length;

                //TODO (Improvment) maxMessage transport size
                /*
                if (_transportMessageSizeSettings.OptimalTransportMessageSize <= compressedPayloadBytes)
                {
                    //TODO log Information splittes messages

                    var numberOfMessages = messages.Count();

                    if (numberOfMessages > 1) 
                    {

                    }
                }
                */

                var transportMessage = new TransportMessage()
                {
                    Topic = groupedOutboxMessagesWithTheSameTopic.Topic,
                    Payload = compressedPayload,
                };

                transportMessages.Add(transportMessage);
            }

            return transportMessages;
        }
    }
}
