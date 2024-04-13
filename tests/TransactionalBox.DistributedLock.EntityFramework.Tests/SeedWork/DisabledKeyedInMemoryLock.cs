using TransactionalBox.KeyedInMemoryLock;

namespace TransactionalBox.DistributedLock.EntityFramework.Tests.SeedWork
{
    internal sealed class DisabledKeyedInMemoryLock : IKeyedInMemoryLock
    {
        public Task<ILockInstance> Acquire(string key, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<ILockInstance>(new DisabledLockInstance());
        }
    }
}
