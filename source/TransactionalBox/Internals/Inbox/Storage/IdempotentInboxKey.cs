namespace TransactionalBox.Internals.Inbox.Storage
{
    internal sealed class IdempotentInboxKey
    {
        public Guid Id { get; }

        public DateTime ExpirationUtc { get; }

        private IdempotentInboxKey() { }

        public IdempotentInboxKey(Guid id, TimeSpan timeToLive, TimeProvider timeProvider)
        {
            var nowUtc = timeProvider.GetUtcNow().UtcDateTime;

            Id = id;
            ExpirationUtc = nowUtc + timeToLive;
        }
    }
}
