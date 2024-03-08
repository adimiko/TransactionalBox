using System.Text.Json;
using TransactionalBox.Outbox.Internals.Exceptions;
using TransactionalBox.OutboxBase.StorageModel;

namespace TransactionalBox.Outbox.Internals
{
    internal sealed class InternalOutbox : IOutbox
    {
        private readonly IOutboxStorage _outbox;

        private readonly TopicFactory _topicFactory;

        public InternalOutbox(IOutboxStorage outbox, TopicFactory topicFactory) 
        {
            _outbox = outbox;
            _topicFactory = topicFactory;
        }

        public async Task Add<TMessage>(TMessage message, string receiver, DateTime occurredUtc) where TMessage : OutboxMessageBase
        {
            if (string.IsNullOrWhiteSpace(receiver)) 
            {
                throw new ReceiverCannotBeNullOrEmptyException();
            }

            if (occurredUtc.Kind != DateTimeKind.Utc)
            {
                throw new OccurredUtcMustBeUtcException();
            }

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(), //TODO Sequential GUID #14
                OccurredUtc = occurredUtc,
                ProcessedUtc = null,
                Topic = _topicFactory.Create(receiver, message),
                Payload = JsonSerializer.Serialize((dynamic)message), //TODO #27
            };

            await _outbox.Add(outboxMessage);
        }
    }
}