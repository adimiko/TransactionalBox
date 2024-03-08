using System.Text.Json;
using TransactionalBox.Outbox.Internals.Exceptions;
using TransactionalBox.OutboxBase.StorageModel;

namespace TransactionalBox.Outbox.Internals
{
    internal sealed class InternalOutbox : IOutbox
    {
        private readonly IOutboxStorage _outboxStorage;

        private readonly TopicFactory _topicFactory;

        public InternalOutbox(
            IOutboxStorage outbox,
            TopicFactory topicFactory) 
        {
            _outboxStorage = outbox;
            _topicFactory = topicFactory;
        }

        public async Task Add<TOutboxMessageBase>(TOutboxMessageBase message, Action<OutboxMessageMetadata>? metadataConfiguration = null)
            where TOutboxMessageBase : OutboxMessageBase
        {
            var metadata = new OutboxMessageMetadata();

            if (metadata.OccurredUtc.Kind != DateTimeKind.Utc)
            {
                throw new OccurredUtcMustBeUtcException();
            }

            var receiver = metadata.Receiver;

            if (receiver is null)
            {
                receiver = "ModuleName"; //TODO ServiceNameProvider
            }

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(), //TODO Sequential GUID #14
                OccurredUtc = metadata.OccurredUtc,
                ProcessedUtc = null,
                Topic = _topicFactory.Create(receiver, message),
                Payload = JsonSerializer.Serialize((dynamic)message), //TODO #27
            };

            await _outboxStorage.Add(outboxMessage);
        }
    }
}