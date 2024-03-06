namespace TransactionalBox.InboxBase.StorageModel
{
    public sealed class InboxMessageStorageModel
    {
        public required Guid Id { get; set; }

        public required DateTime OccurredUtc { get; set; }

        public bool IsProcessed { get; set; }

        public required string Topic { get; set; }

        public required string Payload { get; set; }
    }
}
