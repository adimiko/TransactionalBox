namespace TransactionalBox.OutboxBase.StorageModel
{
    public sealed class OutboxLock
    {
        public string Id { get; set; }

        public DateTime ExpirationUtc { get; set; }

        public string? JobExecutorId { get; set; }

        public bool IsReleased { get; set; } = false;
    }
}
