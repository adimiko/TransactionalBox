using TransactionalBox.KeyedInMemoryLock;

namespace TransactionalBox.DistributedLock.Internals
{
    internal sealed class InternalDistributedLock<T> : IDistributedLock<T>
        where T : Lock, new()
    {
        private static bool _addedFirstLock = false;

        private readonly IDistributedLockStorage _distributedLockStorage;

        private readonly IKeyedInMemoryLock _inMemoryLock;

        private readonly IServiceProvider _serviceProvider;

        public InternalDistributedLock(
            IDistributedLockStorage distributedLockStorage,
            IKeyedInMemoryLock inMemoryLock,
            IServiceProvider serviceProvider) 
        {
            _distributedLockStorage = distributedLockStorage;
            _inMemoryLock = inMemoryLock;
            _serviceProvider = serviceProvider;
        }

        public async Task<IDistributedLockInstance> Acquire(
            string key,
            DateTime nowUtc, //TODO timeprovider
            TimeSpan lockTimeout,
            TimeSpan checkingIntervalWhenLockIsNotReleased)
        {
            var _inMemoryLockInstance = await _inMemoryLock.Acquire(typeof(T).Name);

            if (!_addedFirstLock)
            {
                await AddFirstLock(key, nowUtc, lockTimeout);
                _addedFirstLock = true;
            }

            bool isAcquired = false;

            DateTime expirationUtc;

            do
            {
                var now = DateTime.UtcNow; //TODO

                expirationUtc = CalculateExpirationUtc(now, lockTimeout);

                isAcquired = await _distributedLockStorage.TryAcquire<T>(key, now, expirationUtc);

                if (!isAcquired)
                {
                    await Task.Delay(checkingIntervalWhenLockIsNotReleased);
                }
            }
            while (!isAcquired); //TODO CancellationToken

            var @lock = new T() { Key = key, ExpirationUtc = expirationUtc };

            return new DistributedLockInstance<T>(@lock, _inMemoryLockInstance, _serviceProvider);
        }

        private async Task AddFirstLock(string key, DateTime nowUtc, TimeSpan lockTimeout)
        {
            var @lock = new T()
            {
                Key = key,
                ExpirationUtc = CalculateExpirationUtc(nowUtc,lockTimeout),
            };

            await _distributedLockStorage.AddFirstLock(@lock);
        }

        private DateTime CalculateExpirationUtc(DateTime nowUtc, TimeSpan lockTimeout) => nowUtc + lockTimeout;
    }
}
