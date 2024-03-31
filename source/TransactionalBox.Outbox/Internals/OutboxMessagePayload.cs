namespace TransactionalBox.Outbox.Internals
{
    internal sealed class OutboxMessagePayload<T> : IOutboxMessagePayload
        where T : class, IOutboxMessage
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
