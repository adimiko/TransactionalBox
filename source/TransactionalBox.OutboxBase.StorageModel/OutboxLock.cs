namespace TransactionalBox.OutboxBase.StorageModel
{
    public sealed class OutboxLock
    {
        public string Id { get; set; }

        public DateTime MomentOfAcquireUtc { get; set; }

        public DateTime ExpirationUtc { get; set; }

        public string? JobExecutorId { get; set; }

        public int ConcurrencyToken { get;  set; }

        public bool IsReleased { get; set; } = false;

        public void GenerateNewConcurrencyToken()
        {
            if (ConcurrencyToken == int.MaxValue)
            {
                ConcurrencyToken = 0;
            }
            else
            {
                ConcurrencyToken++;
            }
        }
        //TODO 
    }
}
