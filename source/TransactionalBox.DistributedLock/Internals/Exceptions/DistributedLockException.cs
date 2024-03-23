namespace TransactionalBox.DistributedLock.Internals.Exceptions
{
    internal abstract class DistributedLockException : Exception
    {
        protected DistributedLockException(string message)
            : base(message) { }
    }
}
