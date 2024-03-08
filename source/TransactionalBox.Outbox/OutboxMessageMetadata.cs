namespace TransactionalBox.Outbox
{
    public sealed class OutboxMessageMetadata
    {
        public string? Receiver { get; set; } = null;

        public DateTime OccurredUtc { get; set; } = DateTime.UtcNow; //TODO TimeProvider

        internal OutboxMessageMetadata() { }

        //TODO map to DbMetadata
    }
}
