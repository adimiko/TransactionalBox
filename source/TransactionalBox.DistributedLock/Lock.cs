using TransactionalBox.DistributedLock.Internals.Exceptions;

namespace TransactionalBox.DistributedLock
{
    public abstract class Lock
    {
        public string Key { get; init; }

        public int ConcurrencyToken { get; init; }

        public bool IsReleased { get; private set; } = false;

        public DateTime TimeoutUtc { get; init; }

        public DateTime StartUtc { get; init; }

        public Lock() { }

        public static T CreateFirstLock<T>(
            string key,
            DateTime startUtc,
            DateTime timeoutUtc)
            where T : Lock, new()
        {
            return new T()
            {
                Key = key,
                ConcurrencyToken = 0,
                IsReleased = false,
                TimeoutUtc = timeoutUtc,
                StartUtc = startUtc,
            };
        }

        public T CreateNewLock<T>(DateTime nowUtc, TimeSpan timeout)
            where T : Lock, new()
        {
            var timeoutUtc = nowUtc + timeout;
            var concurrencyToken = GenerateNewConcurrencyToken(this);

            return new T()
            {
                Key = Key,
                ConcurrencyToken = concurrencyToken,
                IsReleased = false,
                TimeoutUtc = timeoutUtc,
                StartUtc = nowUtc,
            };
        }

        public void Release()
        {
            if (IsReleased)
            {
                throw new LockHasAlreadyBeenReleasedException(Key);
            }

            IsReleased = true;
        }

        private static int GenerateNewConcurrencyToken(Lock @lock)
        {
            if (@lock.ConcurrencyToken == int.MaxValue)
            {
                return 0;
            }

            var newToken = @lock.ConcurrencyToken + 1;

            return newToken;
        }
    }
}
