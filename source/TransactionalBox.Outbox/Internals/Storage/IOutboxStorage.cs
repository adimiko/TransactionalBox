namespace TransactionalBox.Outbox.Internals.Storage
{
    internal interface IOutboxStorage
    {
        Task Add(OutboxMessageStorage message);

        Task AddRange(IEnumerable<OutboxMessageStorage> messages);
    }
}
