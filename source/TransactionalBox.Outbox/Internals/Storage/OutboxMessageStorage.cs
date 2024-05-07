namespace TransactionalBox.Outbox.Internals.Storage
{
    internal sealed class OutboxMessageStorage
    {
        public required Guid Id { get; set; }

        public required DateTime OccurredUtc { get; set; }

        public bool IsProcessed { get; set; }

        public required string Topic { get; set; }

        public required string Payload { get; set; }

        public DateTime? LockUtc { get; set; }

        public string? JobId { get; set; } //TODO sequence based on timestamp + machineName + processId
    }
}
