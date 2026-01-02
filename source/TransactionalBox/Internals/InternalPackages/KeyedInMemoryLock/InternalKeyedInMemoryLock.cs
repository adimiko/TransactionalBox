using AsyncKeyedLock;

namespace TransactionalBox.Internals.InternalPackages.KeyedInMemoryLock
{
    internal sealed class InternalKeyedInMemoryLock : IKeyedInMemoryLock
    {
        private static readonly AsyncKeyedLocker<string> _locks = new();

        public async Task<ILockInstance> Acquire(string key, CancellationToken cancellationToken = default)
        {
            return new LockInstance(await _locks.LockAsync(key, cancellationToken));
        }
    }
}
