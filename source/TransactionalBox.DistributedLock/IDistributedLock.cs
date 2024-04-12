namespace TransactionalBox.DistributedLock
{
    public interface IDistributedLock<T>
        where T : Lock, new()
    {
        Task<IDistributedLockInstance> Acquire(string key, DateTime nowUtc, TimeSpan lockTimeout, TimeSpan checkingIntervalWhenLockIsNotReleased);
    }
}
