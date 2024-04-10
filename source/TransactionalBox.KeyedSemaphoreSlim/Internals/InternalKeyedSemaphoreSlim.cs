using System.Collections.Concurrent;

namespace TransactionalBox.KeyedSemaphoreSlim.Internals
{
    internal sealed class InternalKeyedSemaphoreSlim : IKeyedSemaphoreSlim
    {
        private static ConcurrentDictionary<string, SemaphoreSlim> _locks = new ConcurrentDictionary<string, SemaphoreSlim>();

        private SemaphoreSlim? _currentLock;

        public async Task<IKeyedSemaphoreSlim> Acquire(string key, CancellationToken cancellationToken = default)
        {
            _currentLock = _locks.GetOrAdd(key, x => new SemaphoreSlim(1, 1));

            await _currentLock.WaitAsync(cancellationToken);

            return this;
        }

        public void Release() => _currentLock?.Release();

        public void Dispose() => Release();
    }
}
