namespace TransactionalBox.OutboxBase.StorageModel.Internals
{
    internal sealed class OutboxMessage
    {
        public required Guid Id { get; set; }

        public required DateTime OccurredUtc { get; set; }

        public required string Topic { get; set; }

        public required string Payload { get; set; }

        public DateTime? LockUtc { get; set; }

        public DateTime? ProcessedUtc { get; set; }

        public string? JobId { get; set; } //TODO sequence based on timestamp + machineName + processId

    }
}
