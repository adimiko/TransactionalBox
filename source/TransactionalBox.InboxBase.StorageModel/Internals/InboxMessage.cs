namespace TransactionalBox.InboxBase.StorageModel.Internals
{
    internal sealed class InboxMessage
    {
        public required Guid Id { get; set; }

        public DateTime AddedUtc { get; set; }

        public required DateTime OccurredUtc { get; set; }

        public bool IsProcessed { get; set; }

        public required string Topic { get; set; }

        public required string Data { get; set; }
    }
}
