using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Internals.DistributedLock.Internals.Contracts;
using TransactionalBox.Internals.KeyedInMemoryLock;

namespace TransactionalBox.Internals.DistributedLock.Internals
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
            using var scope = _serviceProvider.CreateScope();
            var storage = scope.ServiceProvider.GetRequiredService<IDistributedLockStorage>();
            var nowUtc = TimeProvider.System.GetUtcNow().UtcDateTime;

            var x = await storage.Release<T>(_lock.Key, nowUtc, _lock.ExpirationUtc).ConfigureAwait(false); //TODO logging release

            _inMemoryLockInstance.Dispose();
        }
    }
}
