using TransactionalBox.Outbox.Envelopes;

namespace TransactionalBox.Outbox
{
    public interface IOutbox
    {
        Task Send<TOutboxMessage>(TOutboxMessage message, string receiver, Action<Envelope>? envelopeConfiguration = null)
            where TOutboxMessage : OutboxMessage;

        Task Publish<TOutboxMessage>(TOutboxMessage message, Action<Envelope>? envelopeConfiguration = null)
            where TOutboxMessage : OutboxMessage;
    }
}
