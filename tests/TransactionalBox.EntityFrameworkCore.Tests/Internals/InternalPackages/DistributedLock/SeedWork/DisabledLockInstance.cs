using TransactionalBox.Internals.InternalPackages.KeyedInMemoryLock;

namespace TransactionalBox.EntityFrameworkCore.Tests.Internals.InternalPackages.DistributedLock.SeedWork
{
    internal sealed class DisabledLockInstance : ILockInstance
    {
        public void Dispose() { return; }
    }
}
