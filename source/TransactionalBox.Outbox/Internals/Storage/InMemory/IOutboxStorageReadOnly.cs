namespace TransactionalBox.Outbox.Internals.Storage.InMemory
{
    internal interface IOutboxStorageReadOnly
    {
        IReadOnlyCollection<OutboxMessage> OutboxMessages { get; }
    }
}
