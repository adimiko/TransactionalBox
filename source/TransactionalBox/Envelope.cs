namespace TransactionalBox
{
    public sealed class Envelope
    {
        public string CorrelationId { get; set; } = Guid.NewGuid().ToString();

        internal Envelope() { }
    }
}
