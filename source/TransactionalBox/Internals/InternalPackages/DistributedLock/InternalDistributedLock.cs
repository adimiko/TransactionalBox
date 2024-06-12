using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using TransactionalBox.Internals.InternalPackages.KeyedInMemoryLock;

namespace TransactionalBox.Internals.InternalPackages.DistributedLock
{
    internal sealed class InternalDistributedLock<T> : IDistributedLock<T>
        where T : Lock, new()
    {
        private static ConcurrentDictionary<string, bool> _isKeyAdded = new();

        private readonly IServiceProvider _serviceProvider;

        public InternalDistributedLock(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<IDistributedLockInstance> Acquire(
            string key,
            TimeProvider timeProvider,
            TimeSpan lockTimeout,
            TimeSpan checkingIntervalWhenLockIsNotReleased,
            CancellationToken cancellationToken = default)
        {
            var scope = _serviceProvider.CreateScope();

            var storage = scope.ServiceProvider.GetRequiredService<IDistributedLockStorage>();
            var inMemoryLock = scope.ServiceProvider.GetRequiredService<IKeyedInMemoryLock>();

            var _inMemoryLockInstance = await inMemoryLock.Acquire(key, cancellationToken);

            var isAddedFirstTime = false;

            var isKeyAdded = _isKeyAdded.GetOrAdd(key, x =>
            {
                isAddedFirstTime = true;

                return isAddedFirstTime;
            });

            if (isAddedFirstTime)
            {
                await AddFirstLock(key, timeProvider, storage);
            }

            bool isAcquired = false;

            DateTime expirationUtc;

            do
            {
                cancellationToken.ThrowIfCancellationRequested();

                var nowUtc = timeProvider.GetUtcNow().UtcDateTime;

                expirationUtc = nowUtc + lockTimeout;

                isAcquired = await storage.TryAcquire<T>(key, nowUtc, expirationUtc);

                if (!isAcquired)
                {
                    await Task.Delay(checkingIntervalWhenLockIsNotReleased, timeProvider, cancellationToken);
                }
            }
            while (!isAcquired);

            cancellationToken.ThrowIfCancellationRequested();

            var @lock = new T() { Key = key, ExpirationUtc = expirationUtc };

            scope.Dispose();

            return new DistributedLockInstance<T>(@lock, _inMemoryLockInstance, _serviceProvider);
        }

        private async Task AddFirstLock(string key, TimeProvider timeProvider, IDistributedLockStorage storage)
        {
            var nowUtc = timeProvider.GetUtcNow().UtcDateTime;

            var @lock = new T()
            {
                Key = key,
                ExpirationUtc = nowUtc,
            };

            await storage.AddFirstLock(@lock);
        }
    }
}
