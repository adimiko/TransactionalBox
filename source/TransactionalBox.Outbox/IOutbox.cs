namespace TransactionalBox.Outbox
{
    public interface IOutbox
    {
        Task Add<TOutboxMessage>(TOutboxMessage message, Action<OutboxMessageMetadata>? metadataConfiguration = null)
            where TOutboxMessage : class, IOutboxMessage;
    }
}
