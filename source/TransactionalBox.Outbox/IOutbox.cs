namespace TransactionalBox.Outbox
{
    public interface IOutbox
    {
        Task Add<TOutboxMessage>(TOutboxMessage message, Action<Envelope>? metadataConfiguration = null)
            where TOutboxMessage : class, IOutboxMessage;
    }
}
