using TransactionalBox.Outbox.Envelopes;

namespace TransactionalBox
{
    public interface IOutbox
    {
        Task Add<TOutboxMessage>(TOutboxMessage message, Action<Envelope>? envelopeConfiguration = null)
            where TOutboxMessage : OutboxMessage;
    }
}
