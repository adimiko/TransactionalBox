﻿namespace TransactionalBox.DistributedLock
{
    public abstract class Lock
    {
        public string Key { get; init; }

        public DateTime ExpirationUtc { get; init; }
    }
}