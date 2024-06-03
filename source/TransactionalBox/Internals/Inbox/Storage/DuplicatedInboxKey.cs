namespace TransactionalBox.Internals.Inbox.Storage
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
