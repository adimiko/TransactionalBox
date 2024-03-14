namespace TransactionalBox.OutboxBase.StorageModel
{
    public sealed class OutboxMessage
    {
        public required Guid Id { get; set; }

        public required DateTime OccurredUtc { get; set; }

        public required string Topic { get; set; }

        public required string Payload { get; set; }

        public DateTime? LockUtc { get; set; }

        public DateTime? ProcessedUtc { get; set; }

        public string? ProcessId { get; set; }
    }
}
