namespace TransactionalBox.Outbox.Internals.Storage.InMemory
{
    internal interface IOutboxStorageReadOnly
    {
        IReadOnlyCollection<OutboxMessageStorage> OutboxMessages { get; }
    }
}
