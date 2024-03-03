namespace TransactionalBox.OutboxWorker.Internals
{
    public interface IOutboxRepository
    {
        Task<IEnumerable<OutboxMessage>> GetMessages();
    }
}
