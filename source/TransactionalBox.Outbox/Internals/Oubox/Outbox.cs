using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Internals;
using TransactionalBox.Outbox.Envelopes;
using TransactionalBox.Outbox.Internals.OutboxMessageDefinitions;
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

        private readonly IServiceProvider _serviceProvider;

        public Outbox(
            IServiceContext serviceContext,
            IOutboxStorage outbox,
            IOutboxSerializer serializer,
            ISystemClock systemClock,
            ITopicFactory topicFactory,
            IServiceProvider serviceProvider)
        {
            _serviceContext = serviceContext;
            _outboxStorage = outbox;
            _serializer = serializer;
            _systemClock = systemClock;
            _topicFactory = topicFactory;
            _serviceProvider = serviceProvider;
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

            var messageType = message.GetType();

            IOutboxMessageDefinition? outboxMessageDefinition = _serviceProvider.GetKeyedService<IOutboxMessageDefinition>(messageType);

            if (outboxMessageDefinition is null) 
            {
                outboxMessageDefinition = new DefaultOutboxMessageDefinition();
            }

            string topic;

            if (outboxMessageDefinition.Receiver is not null) 
            {
                topic = _topicFactory.Create(outboxMessageDefinition.Receiver, messageType.Name);
            }
            else
            {
                topic = _topicFactory.Create(_serviceContext.Id, messageType.Name);
            }

            var outboxMessage = new OutboxMessageStorage
            {
                Id = Guid.NewGuid(), //TODO Sequential GUID #14
                OccurredUtc = metadata.OccurredUtc,
                IsProcessed = false,
                Topic = topic,
                Payload = _serializer.Serialize(outboxMessagePayload),
            };

            await _outboxStorage.Add(outboxMessage);
        }

        //TODO AddRange
    }
}