namespace TransactionalBox.Outbox
{
    public sealed class Envelope
    {
        public string? Receiver { get; set; } = null;

        public DateTime OccurredUtc { get; set; } = TimeProvider.System.GetUtcNow().UtcDateTime;

        internal Envelope() { }

        //TODO map to DbMetadata
    }
}
