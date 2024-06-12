using TransactionalBox.Internals.InternalPackages.KeyedInMemoryLock;

namespace TransactionalBox.End2EndTests.Internals.InternalPackages.DistributedLockEntityFrameworkCore.SeedWork
{
    internal sealed class DisabledKeyedInMemoryLock : IKeyedInMemoryLock
    {
        public Task<ILockInstance> Acquire(string key, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<ILockInstance>(new DisabledLockInstance());
        }
    }
}
