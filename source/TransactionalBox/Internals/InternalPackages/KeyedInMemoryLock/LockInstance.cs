﻿namespace TransactionalBox.Internals.InternalPackages.KeyedInMemoryLock
{
    internal sealed class LockInstance : ILockInstance
    {
        private readonly SemaphoreSlim _semaphoreSlim;

        internal LockInstance(SemaphoreSlim semaphoreSlim) => _semaphoreSlim = semaphoreSlim;

        public void Dispose() => _semaphoreSlim.Release();
    }
}
