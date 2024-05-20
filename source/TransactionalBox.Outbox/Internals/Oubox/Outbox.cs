using TransactionalBox.Internals;
using TransactionalBox.Outbox.Envelopes;
using TransactionalBox.Outbox.Internals.Serialization;
using TransactionalBox.Outbox.Internals.Storage;

namespace TransactionalBox.Outbox.Internals.Oubox
{
    internal sealed class Outbox : IOutbox
    {
        private readonly IServiceContext _serviceContext;

        private readonly IOutboxStorage _outboxStorage;

        private readonly IOutboxSerializer _serializer;

        private readonly ISystemClock _systemClock;

        private readonly ITopicFactory _topicFactory;

        public Outbox(
            IServiceContext serviceContext,
            IOutboxStorage outbox,
            IOutboxSerializer serializer,
            ISystemClock systemClock,
            ITopicFactory topicFactory)
        {
            _serviceContext = serviceContext;
            _outboxStorage = outbox;
            _serializer = serializer;
            _systemClock = systemClock;
            _topicFactory = topicFactory;
        }

        public async Task Add<TOutboxMessage>(TOutboxMessage message, Action<Envelope>? envelopeConfiguration = null)
            where TOutboxMessage : OutboxMessage
        {
            var envelope = new Envelope();

            if (envelopeConfiguration is not null)
            {
                envelopeConfiguration(envelope);
            }

            var metadata = new Metadata(envelope.CorrelationId, _serviceContext.Id, _systemClock.UtcNow);

            var outboxMessagePayload = new OutboxMessagePayload<TOutboxMessage>(metadata, message);

            //TODO topic factory based on OutboxMessageDefinition

            var outboxMessage = new OutboxMessageStorage
            {
                Id = Guid.NewGuid(), //TODO Sequential GUID #14
                OccurredUtc = metadata.OccurredUtc,
                IsProcessed = false,
                Topic = _topicFactory.Create(_serviceContext.Id, message.GetType().Name),
                Payload = _serializer.Serialize(outboxMessagePayload),
            };

            await _outboxStorage.Add(outboxMessage);
        }

        //TODO AddRange
    }
}