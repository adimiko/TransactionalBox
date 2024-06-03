namespace TransactionalBox.Internals.Outbox.Storage.InMemory
{
    internal interface IOutboxStorageReadOnly
    {
        IReadOnlyCollection<OutboxMessageStorage> OutboxMessages { get; }
    }
}
