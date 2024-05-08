using TransactionalBox.Outbox.Internals.Serialization;

namespace TransactionalBox.Outbox.Internals.Oubox
{
    internal sealed class OutboxMessagePayload<T> : IOutboxMessagePayload
        where T : OutboxMessage
    {
        public Metadata Metadata { get; }

        public T Message { get; }

        internal OutboxMessagePayload(
            Metadata metadata,
            T message)
        {
            Metadata = metadata;
            Message = message;
        }
    }
}
