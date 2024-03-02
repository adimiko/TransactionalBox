namespace TransactionalBox.Outbox.Internals
{
    public interface IOutboxRepository
    {
        Task Add(OutboxMessage message);
    }
}
