using System.Collections.Concurrent;

namespace TransactionalBox.Internals.KeyedInMemoryLock.Internals
{
    internal sealed class InternalKeyedInMemoryLock : IKeyedInMemoryLock
    {
        private static ConcurrentDictionary<string, SemaphoreSlim> _locks = new ConcurrentDictionary<string, SemaphoreSlim>();

        public async Task<ILockInstance> Acquire(string key, CancellationToken cancellationToken = default)
        {
            var @lock = _locks.GetOrAdd(key, x => new SemaphoreSlim(1, 1));

            await @lock.WaitAsync(cancellationToken);

            return new LockInstance(@lock);
        }
    }
}
