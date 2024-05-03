using TransactionalBox.Outbox.Envelopes;

namespace TransactionalBox.Outbox.Internals.Oubox
{
    internal sealed class Metadata
    {
        public string Source { get; }

        public DateTime OccurredUtc { get; }

        public string CorrelationId { get; }

        internal Metadata(Envelope envelope, string serviceName, DateTime nowUtc)
        {
            Source = serviceName;
            OccurredUtc = nowUtc;
            CorrelationId = envelope.CorrelationId;
        }
    }
}
