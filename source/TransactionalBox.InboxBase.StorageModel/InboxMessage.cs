namespace TransactionalBox.InboxBase.StorageModel
{
    public sealed class InboxMessage
    {
        public required Guid Id { get; set; }

        public required DateTime OccurredUtc { get; set; }

        public DateTime? ProcessedUtc { get; set; }

        public required string Topic { get; set; }

        public required string Payload { get; set; }
    }
}
