using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.DistributedLock.Internals.Contracts;
using TransactionalBox.KeyedInMemoryLock;

namespace TransactionalBox.DistributedLock.Internals
{
    internal sealed class DistributedLockInstance<T> : IDistributedLockInstance
        where T : Lock, new()
    {
        private readonly T _lock;

        private readonly ILockInstance _inMemoryLockInstance;

        private readonly IServiceProvider _serviceProvider;

        internal DistributedLockInstance(
            T @lock,
            ILockInstance inMemoryLockInstance,
            IServiceProvider serviceProvider) 
        {
            _lock = @lock;
            _inMemoryLockInstance = inMemoryLockInstance;
            _serviceProvider = serviceProvider;
        }

        public async ValueTask DisposeAsync()
        {
            var storage = _serviceProvider.GetRequiredService<IDistributedLockStorage>();
            var nowUtc = TimeProvider.System.GetUtcNow().UtcDateTime;

            var x = await storage.Release<T>(_lock.Key, nowUtc, _lock.ExpirationUtc); //TODO logging release

            _inMemoryLockInstance.Dispose();
        }
    }
}
