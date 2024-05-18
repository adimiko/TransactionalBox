using TransactionalBox.Internals.KeyedInMemoryLock;

namespace TransactionalBox.Internals.DistributedLock.EntityFrameworkCore.Tests.SeedWork
{
    internal sealed class DisabledLockInstance : ILockInstance
    {
        public void Dispose() { return; }
    }
}
