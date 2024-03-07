namespace TransactionalBox.Outbox
{
    public interface IOutboxSender
    {
        Task Send<TOutboxMessageBase>(TOutboxMessageBase message, string receiver, DateTime occurredUtc)
            where TOutboxMessageBase : OutboxMessageBase;
    }
}
