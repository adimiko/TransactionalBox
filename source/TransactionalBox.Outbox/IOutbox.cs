using TransactionalBox.Outbox.Envelopes;

namespace TransactionalBox.Outbox
{
    public interface IOutbox
    {
        Task Add<TOutboxMessage>(TOutboxMessage message, Action<Envelope>? envelopeConfiguration = null)
            where TOutboxMessage : class, IOutboxMessage;

        Task AddRange<TOutboxMessage>(IEnumerable<TOutboxMessage> messages, Action<Envelope>? envelopeConfiguration = null)
            where TOutboxMessage : class, IOutboxMessage;
    }
}
