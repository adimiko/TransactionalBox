namespace TransactionalBox.Internals.InternalPackages.DistributedLock
{
    internal abstract class Lock
    {
        public string Key { get; init; }

        public DateTime ExpirationUtc { get; init; }
    }
}
