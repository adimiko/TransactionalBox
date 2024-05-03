namespace TransactionalBox.Outbox.Internals.Storage
{
    internal interface IOutboxStorage
    {
        Task Add(OutboxMessage message);

        Task AddRange(IEnumerable<OutboxMessage> messages);
    }
}
