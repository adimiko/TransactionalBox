namespace TransactionalBox.Internals.InternalPackages.KeyedInMemoryLock
{
    internal interface IKeyedInMemoryLock
    {
        Task<ILockInstance> Acquire(string key, CancellationToken cancellationToken = default);
    }
}
