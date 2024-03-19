namespace TransactionalBox.InboxBase.StorageModel
{
    public sealed class IdempotentInboxMessage
    {
        public Guid Id { get; }

        public DateTime AddedUtc { get; }

        private IdempotentInboxMessage(Guid id, DateTime addedUtc) 
        {
            Id = id;
            AddedUtc = addedUtc;
        }

        public static IdempotentInboxMessage CreateBasedOnInboxMessage(InboxMessage inboxMessage)
        {
            return new IdempotentInboxMessage(inboxMessage.Id, inboxMessage.AddedUtc);
        }
    }
}
