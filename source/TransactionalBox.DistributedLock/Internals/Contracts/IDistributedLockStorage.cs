namespace TransactionalBox.DistributedLock.Internals.Contracts
{
    internal interface IDistributedLockStorage
    {
        Task AddFirstLock<T>(T @lock) where T : Lock, new();

        Task<bool> TryAcquire<T>(string key, DateTime nowUtc, DateTime newExpirationUtc) where T : Lock, new();

        Task<bool> Release<T>(string key, DateTime nowUtc, DateTime expirationUtc) where T : Lock, new();
    }
}
