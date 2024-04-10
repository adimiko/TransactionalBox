using TransactionalBox.KeyedInMemoryLock.Internals;

namespace TransactionalBox.KeyedInMemoryLock
{
    public interface IKeyedInMemoryLock : IDisposable
    {
        Task<IKeyedInMemoryLock> Acquire(string key, CancellationToken cancellationToken = default);

        void Release();
    }
}
