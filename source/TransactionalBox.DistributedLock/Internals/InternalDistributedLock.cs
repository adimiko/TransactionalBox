using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.DistributedLock.Internals.Contracts;
using TransactionalBox.KeyedInMemoryLock;

namespace TransactionalBox.DistributedLock.Internals
{
    internal sealed class InternalDistributedLock<T> : IDistributedLock<T>
        where T : Lock, new()
    {
        private static bool _addedFirstLock = false;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public InternalDistributedLock(IServiceScopeFactory serviceScopeFactory) 
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<IDistributedLockInstance> Acquire(
            string key,
            TimeProvider timeProvider,
            TimeSpan lockTimeout,
            TimeSpan checkingIntervalWhenLockIsNotReleased, 
            CancellationToken cancellationToken = default)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var storage = scope.ServiceProvider.GetRequiredService<IDistributedLockStorage>();
            var inMemoryLock = scope.ServiceProvider.GetRequiredService<IKeyedInMemoryLock>();

            var _inMemoryLockInstance = await inMemoryLock.Acquire(key, cancellationToken);

            if (!_addedFirstLock)
            {
                await AddFirstLock(key, timeProvider, lockTimeout, storage);
                _addedFirstLock = true;
            }

            bool isAcquired = false;

            DateTime expirationUtc;

            do
            {
                var nowUtc = timeProvider.GetUtcNow().UtcDateTime;

                expirationUtc = CalculateExpirationUtc(nowUtc, lockTimeout);

                isAcquired = await storage.TryAcquire<T>(key, nowUtc, expirationUtc);

                if (!isAcquired)
                {
                    await Task.Delay(checkingIntervalWhenLockIsNotReleased);
                }
            }
            while (!isAcquired || cancellationToken.IsCancellationRequested);

            cancellationToken.ThrowIfCancellationRequested();

            var @lock = new T() { Key = key, ExpirationUtc = expirationUtc };

            return new DistributedLockInstance<T>(@lock, _inMemoryLockInstance, _serviceScopeFactory);
        }

        private async Task AddFirstLock(string key, TimeProvider timeProvider, TimeSpan lockTimeout, IDistributedLockStorage storage)
        {
            var nowUtc = timeProvider.GetUtcNow().UtcDateTime;

            var @lock = new T()
            {
                Key = key,
                ExpirationUtc = CalculateExpirationUtc(nowUtc,lockTimeout),
            };

            await storage.AddFirstLock(@lock);
        }

        private DateTime CalculateExpirationUtc(DateTime nowUtc, TimeSpan lockTimeout) => nowUtc + lockTimeout;
    }
}
