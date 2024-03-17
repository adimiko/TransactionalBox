namespace TransactionalBox.OutboxBase.StorageModel
{
    public sealed class OutboxLock
    {
        public string Key { get; set; }

        public DateTime StartUtc { get; set; }

        public DateTime TimeoutUtc { get; set; }

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
