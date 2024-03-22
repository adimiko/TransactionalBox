namespace TransactionalBox.InboxBase.StorageModel.Internals
{
    internal sealed class IdempotentInboxKey
    {
        public Guid Id { get; }

        public DateTime AddedUtc { get; }

        private IdempotentInboxKey() { }

        private IdempotentInboxKey(Guid id, DateTime addedUtc)
        {
            Id = id;
            AddedUtc = addedUtc;
        }

        public static IdempotentInboxKey CreateBasedOnInboxMessage(InboxMessage inboxMessage)
        {
            return new IdempotentInboxKey(inboxMessage.Id, inboxMessage.AddedUtc);
        }
    }
}
