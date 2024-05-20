using TransactionalBox.Internals.KeyedInMemoryLock;

namespace TransactionalBox.Internals.DistributedLock.EntityFrameworkCore.Tests.SeedWork
{
    internal sealed class DisabledKeyedInMemoryLock : IKeyedInMemoryLock
    {
        public Task<ILockInstance> Acquire(string key, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<ILockInstance>(new DisabledLockInstance());
        }
    }
}
