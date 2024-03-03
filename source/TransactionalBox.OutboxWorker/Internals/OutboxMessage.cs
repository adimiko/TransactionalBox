namespace TransactionalBox.OutboxWorker.Internals
{
    public sealed class OutboxMessage
    {
        public required Guid Id { get; init; }

        public required DateTime OccurredUtc { get; init; }

        public DateTime? ProcessedUtc { get; init; }

        public required string Topic { get; init; }

        public required string Payload { get; init; }
    }
}
