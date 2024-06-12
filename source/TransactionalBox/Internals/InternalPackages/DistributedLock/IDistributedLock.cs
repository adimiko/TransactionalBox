namespace TransactionalBox.Internals.InternalPackages.DistributedLock
{
    internal interface IDistributedLock<T>
        where T : Lock, new()
    {
        Task<IDistributedLockInstance> Acquire(string key, TimeProvider timeProvider, TimeSpan lockTimeout, TimeSpan checkingIntervalWhenLockIsNotReleased, CancellationToken cancellationToken = default);
    }
}
