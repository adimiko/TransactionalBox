using TransactionalBox.KeyedSemaphoreSlim.Internals;

namespace TransactionalBox.KeyedSemaphoreSlim
{
    public interface IKeyedSemaphoreSlim : IDisposable
    {
        Task<IKeyedSemaphoreSlim> Acquire(string key, CancellationToken cancellationToken = default);

        void Release();
    }
}
