namespace TransactionalBox.DistributedLock.Internals
{
    internal sealed class InternalDistributedLock<T> : IDistributedLock<T>
        where T : Lock, new()
    {
        private static bool _addedFirstLock = false;

        private readonly IDistributedLockStorage _distributedLockStorage;

        private T _newLock;

        public InternalDistributedLock(IDistributedLockStorage distributedLockStorage) 
        {
            _distributedLockStorage = distributedLockStorage;
        }

        public async Task Acquire(
            string key,
            DateTime nowUtc,
            TimeSpan lockTimeout,
            TimeSpan checkingIntervalWhenLockIsNotReleased)
        {
            if (!_addedFirstLock)
            {
                await AddFirstLock(key, nowUtc, lockTimeout);
                _addedFirstLock = true;
            }

            bool isLockAcquired = false;

            do
            {
                var previousReleasedLock = await _distributedLockStorage.GetPreviousReleasedLock<T>(key, nowUtc);

                if (previousReleasedLock is not null)
                {
                    _newLock = previousReleasedLock.CreateNewLock<T>(nowUtc, lockTimeout);

                    isLockAcquired = await _distributedLockStorage.TryAddNextLock(_newLock, previousReleasedLock.ConcurrencyToken);
                }
                else
                {
                    await Task.Delay(checkingIntervalWhenLockIsNotReleased);
                }
            }
            while (!isLockAcquired);
        }

        public async Task Release()
        {
            _newLock.Release();

            var isReleased = await _distributedLockStorage.Release(_newLock);

            if (!isReleased) 
            {
                //TODO LOG
            }
        }

        private async Task AddFirstLock(string key, DateTime nowUtc, TimeSpan lockTimeout)
        {
            var @lock = Lock.CreateFirstLock<T>(key, nowUtc, lockTimeout);

            await _distributedLockStorage.AddFirstLock(@lock);
        }
    }
}
