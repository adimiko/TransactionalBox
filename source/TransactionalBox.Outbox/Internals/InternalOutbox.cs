using System.Text.Json;
using TransactionalBox.Outbox.Internals.Exceptions;
using TransactionalBox.OutboxBase.StorageModel.Internals;

namespace TransactionalBox.Outbox.Internals
{
    internal sealed class InternalOutbox : IOutbox
    {
        private readonly IServiceContext _serviceContext;

        private readonly IOutboxStorage _outboxStorage;

        private readonly TopicFactory _topicFactory;

        public InternalOutbox(
            IServiceContext serviceContext,
            IOutboxStorage outbox,
            TopicFactory topicFactory) 
        {
            _serviceContext = serviceContext;
            _outboxStorage = outbox;
            _topicFactory = topicFactory;
        }

        public async Task Add<TOutboxMessage>(TOutboxMessage message, Action<Envelope>? envelopeConfiguration = null)
            where TOutboxMessage : class, IOutboxMessage
        {
            var envelope = new Envelope();

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
                Data = JsonSerializer.Serialize((dynamic)message), //TODO #27
            };

            await _outboxStorage.Add(outboxMessage);
        }
    }
}