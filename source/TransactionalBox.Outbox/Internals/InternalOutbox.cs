using System.Text.Json;
using TransactionalBox.Internals;
using TransactionalBox.Outbox.Internals.Exceptions;
using TransactionalBox.Outbox.Serialization;
using TransactionalBox.OutboxBase.StorageModel.Internals;

namespace TransactionalBox.Outbox.Internals
{
    internal sealed class InternalOutbox : IOutbox
    {
        private readonly IServiceContext _serviceContext;

        private readonly IOutboxStorage _outboxStorage;

        private readonly IOutboxSerializer _serializer;

        private readonly TopicFactory _topicFactory;

        public InternalOutbox(
            IServiceContext serviceContext,
            IOutboxStorage outbox,
            IOutboxSerializer serializer,
            TopicFactory topicFactory) 
        {
            _serviceContext = serviceContext;
            _outboxStorage = outbox;
            _serializer = serializer;
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

            if (envelope.OccurredUtc.Kind != DateTimeKind.Utc)
            {
                throw new OccurredUtcMustBeUtcException();
            }

            var receiver = envelope.Receiver;

            if (receiver is null)
            {
                receiver = _serviceContext.Id;
            }

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(), //TODO Sequential GUID #14
                OccurredUtc = envelope.OccurredUtc,
                ProcessedUtc = null,
                Topic = _topicFactory.Create(receiver, message),
                Data = _serializer.Serialize(message),
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

            if (envelope.OccurredUtc.Kind != DateTimeKind.Utc)
            {
                throw new OccurredUtcMustBeUtcException();
            }

            var receiver = envelope.Receiver;

            if (receiver is null)
            {
                receiver = _serviceContext.Id;
            }

            var outboxMessages = new List<OutboxMessage>();

            foreach (var message in messages) 
            {
                var outboxMessage = new OutboxMessage
                {
                    Id = Guid.NewGuid(), //TODO Sequential GUID #14
                    OccurredUtc = envelope.OccurredUtc,
                    ProcessedUtc = null,
                    Topic = _topicFactory.Create(receiver, message),
                    Data = _serializer.Serialize(message),
                };

                outboxMessages.Add(outboxMessage);
            }

            await _outboxStorage.AddRange(outboxMessages);
        }
    }
}