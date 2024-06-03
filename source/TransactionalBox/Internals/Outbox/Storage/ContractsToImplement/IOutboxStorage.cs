namespace TransactionalBox.Internals.Outbox.Storage.ContractsToImplement
{
    internal interface IOutboxStorage
    {
        Task Add(OutboxMessageStorage message);

        Task AddRange(IEnumerable<OutboxMessageStorage> messages);
    }
}
