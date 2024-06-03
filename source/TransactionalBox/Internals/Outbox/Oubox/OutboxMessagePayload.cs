using TransactionalBox.Internals.Outbox.Serialization;

namespace TransactionalBox.Internals.Outbox.Oubox
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
