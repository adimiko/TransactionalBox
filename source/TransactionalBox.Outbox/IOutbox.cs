namespace TransactionalBox.Outbox
{
    public interface IOutbox
    {
        Task Add<TOutboxMessageBase>(TOutboxMessageBase message, Action<OutboxMessageMetadata>? metadataConfiguration = null)
            where TOutboxMessageBase : OutboxMessageBase;
    }
}
