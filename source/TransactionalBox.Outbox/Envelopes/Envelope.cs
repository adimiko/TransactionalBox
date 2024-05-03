namespace TransactionalBox.Outbox.Envelopes
{
    public sealed class Envelope
    {
        public string? Receiver { get; set; } = null;

        public string CorrelationId { get; set; } = Guid.NewGuid().ToString();

        internal Envelope() { }
    }
}
