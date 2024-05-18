using TransactionalBox.Internals.KeyedInMemoryLock.Internals;

namespace TransactionalBox.Internals.KeyedInMemoryLock
{
    public interface IKeyedInMemoryLock
    {
        Task<ILockInstance> Acquire(string key, CancellationToken cancellationToken = default);
    }
}
