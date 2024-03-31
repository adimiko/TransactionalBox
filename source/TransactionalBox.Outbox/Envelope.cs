namespace TransactionalBox.Outbox
{
    public sealed class Envelope
    {
        public string? Receiver { get; set; } = null;

        public DateTime OccurredUtc { get; } = TimeProvider.System.GetUtcNow().UtcDateTime;

        public string CorrelationId { get; set; } = Guid.NewGuid().ToString();

        internal Envelope() { }

        //TODO map to DbMetadata
    }
}
