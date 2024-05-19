namespace TransactionalBox.Internals.DistributedLock
{
    internal abstract class Lock
    {
        public string Key { get; init; }

        public DateTime ExpirationUtc { get; init; }
    }
}
