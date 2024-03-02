using System.Text.Json;
using TransactionalBox.Outbox.Internals.Exceptions;

namespace TransactionalBox.Outbox.Internals
{
    internal sealed class OutboxSender : IOutboxSender
    {
        private readonly IOutboxRepository _outbox;

        public OutboxSender(IOutboxRepository outbox) 
        {
            _outbox = outbox;
        }

        public async Task Send<TMessage>(TMessage message, string receiver, DateTime occurredUtc) where TMessage : MessageBase
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
                LockUtc = null,
                ProcessedUtc = null,
                Topic = receiver + "-" + message.GetType().Name,
                Payload = JsonSerializer.Serialize((dynamic)message),
            };

            await _outbox.Add(outboxMessage);
        }
    }
}