﻿using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Internals;
using TransactionalBox.Internals.EventHooks;
using TransactionalBox.Internals.SequentialGuid;
using TransactionalBox.Outbox.Envelopes;
using TransactionalBox.Outbox.Internals.Hooks.Events;
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

        private readonly ISequentialGuidGenerator _sequentialGuidGenerator;

        private readonly IEventHookPublisher _eventHookPublisher;

        public Outbox(
            IServiceContext serviceContext,
            IOutboxStorage outbox,
            IOutboxSerializer serializer,
            ISystemClock systemClock,
            ITopicFactory topicFactory,
            IServiceProvider serviceProvider,
            ISequentialGuidGenerator sequentialGuidGenerator,
            IEventHookPublisher eventHookPublisher)
        {
            _serviceContext = serviceContext;
            _outboxStorage = outbox;
            _serializer = serializer;
            _systemClock = systemClock;
            _topicFactory = topicFactory;
            _serviceProvider = serviceProvider;
            _sequentialGuidGenerator = sequentialGuidGenerator;
            _eventHookPublisher = eventHookPublisher;
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
                Id = _sequentialGuidGenerator.Create(), //TODO Sequential GUID #14
                OccurredUtc = metadata.OccurredUtc,
                IsProcessed = false,
                Topic = topic,
                Payload = _serializer.Serialize(outboxMessagePayload),
            };

            await _outboxStorage.Add(outboxMessage);
        }

        public async Task TransactionCommited()
        {
            await _eventHookPublisher.PublishAsync<AddedMessagesToOutbox>().ConfigureAwait(false);
        }

        //TODO AddRange
    }
}