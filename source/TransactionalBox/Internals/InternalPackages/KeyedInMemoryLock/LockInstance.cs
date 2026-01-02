namespace TransactionalBox.Internals.InternalPackages.KeyedInMemoryLock
{
    internal readonly struct LockInstance : ILockInstance
    {
        private readonly IDisposable _instance;

        internal LockInstance(IDisposable instance) => _instance = instance;

        public void Dispose() => _instance.Dispose();
    }
}
