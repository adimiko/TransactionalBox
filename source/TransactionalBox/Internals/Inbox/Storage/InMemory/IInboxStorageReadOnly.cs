namespace TransactionalBox.Internals.Inbox.Storage.InMemory
{
    internal interface IInboxStorageReadOnly
    {
        IReadOnlyCollection<InboxMessageStorage> InboxMessages { get; }

        IReadOnlyCollection<IdempotentInboxKey> IdempotentInboxKeys { get; }
    }
}
