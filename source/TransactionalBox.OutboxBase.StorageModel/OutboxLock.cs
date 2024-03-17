using TransactionalBox.OutboxBase.StorageModel.Exceptions;

namespace TransactionalBox.OutboxBase.StorageModel
{
    public sealed class OutboxLock
    {
        public string Key { get; private set; }

        public int ConcurrencyToken { get; private set; }

        public bool IsReleased { get; private set; }

        public DateTime TimeoutUtc { get; private set; }

        public DateTime StartUtc { get; private set; }

        private OutboxLock(
            string key,
            int concurrencyToken,
            bool isReleased,
            DateTime timeoutUtc,
            DateTime startUtc)
        {
            Key = key;
            ConcurrencyToken = concurrencyToken;
            IsReleased = isReleased;
            TimeoutUtc = timeoutUtc;
            StartUtc = startUtc;
        }

        public static OutboxLock CreateFirstLock(
            string key,
            DateTime startUtc,
            DateTime timeoutUtc)
        {
            return new OutboxLock(key, 0, false, timeoutUtc, startUtc);
        }

        public OutboxLock CreateNewLock(DateTime nowUtc, TimeSpan timeout)
        {
            var timeoutUtc = nowUtc + timeout;
            var concurrencyToken = GenerateNewConcurrencyToken(this);

            return new OutboxLock(Key, concurrencyToken, false, timeoutUtc, nowUtc);
        }

        public void Release()
        {
            if (IsReleased) 
            {
                throw new LockHasAlreadyBeenReleasedException(Key);
            }

            IsReleased = true;
        }

        private static int GenerateNewConcurrencyToken(OutboxLock outboxLock)
        {
            if (outboxLock.ConcurrencyToken == int.MaxValue)
            {
                return 0;
            }

            var newToken = outboxLock.ConcurrencyToken + 1;

            return newToken;
        }
    }
}
