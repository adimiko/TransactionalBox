namespace TransactionalBox.Outbox
{
    public sealed class OutboxMessageMetadata
    {
        public string? Receiver { get; set; } = null;

        public DateTime OccurredUtc { get; set; } = TimeProvider.System.GetUtcNow().UtcDateTime;

        internal OutboxMessageMetadata() { }

        //TODO map to DbMetadata
    }
}
