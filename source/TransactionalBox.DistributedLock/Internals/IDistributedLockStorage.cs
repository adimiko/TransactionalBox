namespace TransactionalBox.DistributedLock.Internals
{
    internal interface IDistributedLockStorage
    {
        Task<T?> GetPreviousReleasedLock<T>(string key, DateTime nowUtc) where T : Lock, new();

        Task AddFirstLock<T>(T @lock) where T : Lock, new();

        Task<bool> TryAddNextLock<T>(T @lock, int previousLockConcurrencyToken) where T : Lock, new();

        Task<bool> Release<T>(T @lock) where T : Lock, new();
    }
}
