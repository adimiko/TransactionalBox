using TransactionalBox.KeyedInMemoryLock;

namespace TransactionalBox.DistributedLock.EntityFramework.Tests.SeedWork
{
    internal sealed class DisabledLockInstance : ILockInstance
    {
        public void Dispose() { return; }
    }
}
