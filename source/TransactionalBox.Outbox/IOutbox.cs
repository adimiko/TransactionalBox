using TransactionalBox.Outbox.Envelopes;

namespace TransactionalBox
{
    public interface IOutbox
    {
        /// <summary>
        /// Add a message to the outbox with the transaction.
        /// </summary>
        Task Add<TOutboxMessage>(TOutboxMessage message, Action<Envelope>? envelopeConfiguration = null)
            where TOutboxMessage : OutboxMessage;

        Task TransactionCommited();
    }
}
