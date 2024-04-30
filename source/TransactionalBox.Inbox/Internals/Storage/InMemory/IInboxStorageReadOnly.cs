namespace TransactionalBox.Inbox.Internals.Storage.InMemory
{
    internal interface IInboxStorageReadOnly
    {
        IReadOnlyCollection<InboxMessageStorage> InboxMessages { get; }

        IReadOnlyCollection<IdempotentInboxKey> IdempotentInboxKeys { get; }
    }
}
