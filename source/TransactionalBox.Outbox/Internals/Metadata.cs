namespace TransactionalBox.Outbox.Internals
{
    public sealed class Metadata
    {
        public string Source { get; }

        public DateTime OccurredUtc { get; }

        public string CorrelationId { get; }

        internal Metadata(Envelope envelope, string serviceName) 
        {
            Source = serviceName;
            OccurredUtc = envelope.OccurredUtc;
            CorrelationId = envelope.CorrelationId;
        }
    }
}
