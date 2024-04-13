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

        private readonly IServiceScopeFactory _serviceScopeFactory;

        internal DistributedLockInstance(
            T @lock,
            ILockInstance inMemoryLockInstance,
            IServiceScopeFactory serviceScopeFactory) 
        {
            _lock = @lock;
            _inMemoryLockInstance = inMemoryLockInstance;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async ValueTask DisposeAsync()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var storage = scope.ServiceProvider.GetRequiredService<IDistributedLockStorage>();
            var nowUtc = TimeProvider.System.GetUtcNow().UtcDateTime;

            var x = await storage.Release<T>(_lock.Key, nowUtc, _lock.ExpirationUtc); //TODO logging release

            _inMemoryLockInstance.Dispose();
        }
    }
}
