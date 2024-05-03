namespace TransactionalBox.Inbox.Internals.Storage
{
    internal sealed record DuplicatedInboxKey
    {
        internal Guid Id { get; }

        internal DuplicatedInboxKey(IdempotentInboxKey inboxMessage)
        {
            Id = inboxMessage.Id;
        }
    }
}
