namespace TransactionalBox.Outbox
{
    public interface IOutbox
    {
        Task Add<TOutboxMessageBase>(TOutboxMessageBase message, string receiver, DateTime occurredUtc)
            where TOutboxMessageBase : OutboxMessageBase;
    }
}
