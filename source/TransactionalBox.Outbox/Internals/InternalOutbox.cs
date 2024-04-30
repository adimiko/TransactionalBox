using TransactionalBox.Internals;
using TransactionalBox.Base.Outbox.StorageModel.Internals;
using TransactionalBox.Outbox.Internals.Serialization;

namespace TransactionalBox.Outbox.Internals
{
    internal sealed class InternalOutbox : IOutbox
    {
        private readonly IServiceContext _serviceContext;

        private readonly IOutboxStorage _outboxStorage;

        private readonly IOutboxSerializer _serializer;

        private readonly ISystemClock _systemClock;

        private readonly TopicFactory _topicFactory;

        public InternalOutbox(
            IServiceContext serviceContext,
            IOutboxStorage outbox,
            IOutboxSerializer serializer,
            ISystemClock systemClock,
            TopicFactory topicFactory) 
        {
            _serviceContext = serviceContext;
            _outboxStorage = outbox;
            _serializer = serializer;
            _systemClock = systemClock;
            _topicFactory = topicFactory;
        }

        public async Task Add<TOutboxMessage>(TOutboxMessage message, Action<Envelope>? envelopeConfiguration = null)
            where TOutboxMessage : class, IOutboxMessage
        {
            var envelope = new Envelope();

            if (envelopeConfiguration is not null)
            {
                envelopeConfiguration(envelope);
            }

            var receiver = envelope.Receiver;

            if (receiver is null)
            {
                receiver = _serviceContext.Id;
            }

            var metadata = new Metadata(envelope, _serviceContext.Id, _systemClock.UtcNow);
            var outboxMessagePayload = new OutboxMessagePayload<TOutboxMessage>(metadata, message);

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(), //TODO Sequential GUID #14
                OccurredUtc = metadata.OccurredUtc,
                IsProcessed = false,
                Topic = _topicFactory.Create(receiver, message),
                Payload = _serializer.Serialize(outboxMessagePayload),
            };

            await _outboxStorage.Add(outboxMessage);
        }

        public async Task AddRange<TOutboxMessage>(IEnumerable<TOutboxMessage> messages, Action<Envelope>? envelopeConfiguration)
            where TOutboxMessage : class, IOutboxMessage
        {
            var envelope = new Envelope();

            if (envelopeConfiguration is not null)
            {
                envelopeConfiguration(envelope);
            }

            var receiver = envelope.Receiver;

            if (receiver is null)
            {
                receiver = _serviceContext.Id;
            }

            var outboxMessages = new List<OutboxMessage>();

            var metadata = new Metadata(envelope, _serviceContext.Id, _systemClock.UtcNow);

            foreach (var message in messages) 
            {
                var outboxMessagePayload = new OutboxMessagePayload<TOutboxMessage>(metadata, message);

                var outboxMessage = new OutboxMessage
                {
                    Id = Guid.NewGuid(), //TODO Sequential GUID #14
                    OccurredUtc = metadata.OccurredUtc,
                    IsProcessed = false,
                    Topic = _topicFactory.Create(receiver, message),
                    Payload = _serializer.Serialize(outboxMessagePayload),
                };

                outboxMessages.Add(outboxMessage);
            }

            await _outboxStorage.AddRange(outboxMessages);
        }
    }
}