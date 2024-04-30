namespace TransactionalBox.Inbox.Internals.Deserialization
{
    internal sealed class Metadata
    {
        public string Source { get; init; }

        public DateTime OccurredUtc { get; init; }

        public string CorrelationId { get; init; }
    }
}
