using System.Text.Json;

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
            //TODO own exceptions
            ArgumentNullException.ThrowIfNull(message, nameof(message));
            ArgumentException.ThrowIfNullOrWhiteSpace(receiver, nameof(receiver));

            if (occurredUtc.Kind != DateTimeKind.Utc)
            {
                throw new Exception("occurredUtc must be UTC");
            }

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(), //TODO sequential
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