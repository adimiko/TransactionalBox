namespace TransactionalBox.Internals.Outbox.Oubox
{
    internal sealed class Metadata
    {
        public string Source { get; }

        public DateTime OccurredUtc { get; }

        public string CorrelationId { get; }

        internal Metadata(string correlationId, string serviceName, DateTime nowUtc)
        {
            Source = serviceName;
            OccurredUtc = nowUtc;
            CorrelationId = correlationId;
        }
    }
}
