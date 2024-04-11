using TransactionalBox.KeyedInMemoryLock.Internals;

namespace TransactionalBox.KeyedInMemoryLock
{
    public interface IKeyedInMemoryLock
    {
        Task<ILockInstance> Acquire(string key, CancellationToken cancellationToken = default);
    }
}
