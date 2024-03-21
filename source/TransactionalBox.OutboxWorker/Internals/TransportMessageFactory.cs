using System.Text.Json;
using TransactionalBox.OutboxBase.StorageModel;

namespace TransactionalBox.OutboxWorker.Internals
{
    internal sealed class TransportMessageFactory
    {
        // TODO maxLimit for transport
        // TODO TransportSerializer
        // TODO Remove TransactionalBox.TransportBase.Models
        public IEnumerable<TransportMessage> Create(IEnumerable<OutboxMessage> outboxMessages)
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
                //TODO #27
                //var payload = JsonSerializer.Serialize(messages); //TODO not all values from outboxMessage (convert to some new object)

                //TODO if serialized value is bigger that limit, split to 2 transport message

                //TODO settings ?
                //TODO per transport ??
                /*
                const int maxBytes = 1_000_000;
                var payloadBytes = Encoding.UTF8.GetByteCount(payload);
                
                if (maxBytes <= payloadBytes)
                {
                    //TODO log Information splittes messages

                    var numberOfMessages = messages.Count();

                    // splitvar firstTransportMessage = messages.Get
                    // split
                }
                */

                var transportMessage = new TransportMessage()
                {
                    Topic = groupedOutboxMessagesWithTheSameTopic.Topic,
                    Payload = JsonSerializer.Serialize(groupedOutboxMessagesWithTheSameTopic.Messages),
                };

                transportMessages.Add(transportMessage);
            }

            return transportMessages;
        }
    }
}
