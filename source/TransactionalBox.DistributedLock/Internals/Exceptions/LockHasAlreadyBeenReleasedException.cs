namespace TransactionalBox.DistributedLock.Internals.Exceptions
{
    internal sealed class LockHasAlreadyBeenReleasedException : DistributedLockException
    {
        internal LockHasAlreadyBeenReleasedException(string key)
            : base("Key: " + key) { }
    }
}
