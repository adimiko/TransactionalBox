using TransactionalBox.Internals.InternalPackages.KeyedInMemoryLock;

namespace TransactionalBox.End2EndTests.Internals.InternalPackages.DistributedLockEntityFrameworkCore.SeedWork
{
    internal sealed class DisabledLockInstance : ILockInstance
    {
        public void Dispose() { return; }
    }
}
