using System.Collections.Concurrent;

namespace TransactionalBox.KeyedSemaphoreSlim.Internals
{
    //TODO with using and Dispose
    internal sealed class InternalKeyedSemaphoreSlim : IKeyedSemaphoreSlim
    {
        private static ConcurrentDictionary<string, SemaphoreSlim> _locks = new ConcurrentDictionary<string, SemaphoreSlim>();

        private SemaphoreSlim? _currentLock;

        public async Task Acquire(string key, CancellationToken cancellationToken = default)
        {
            _currentLock = _locks.GetOrAdd(key, x => new SemaphoreSlim(1, 1));

            await _currentLock.WaitAsync(cancellationToken);
        }

        public void Release() 
        {
            _currentLock?.Release();
        }
    }
}
