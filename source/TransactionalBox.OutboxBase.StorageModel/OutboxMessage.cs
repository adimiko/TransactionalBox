namespace TransactionalBox.OutboxBase.StorageModel
{
    public sealed class OutboxMessage
    {
        public Guid Id { get; set; }

        public DateTime OccurredUtc { get; set; }

        public DateTime? ProcessedUtc { get; set; }

        public string Topic { get; set; }

        public string Payload { get; set; }
    }
}
