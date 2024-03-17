using TransactionalBox.Internals;

namespace TransactionalBox.OutboxBase.StorageModel.Exceptions
{
    internal sealed class LockHasAlreadyBeenReleasedException : TransactionalBoxException
    {
        internal LockHasAlreadyBeenReleasedException(string key) 
            : base("Key: " + key) { }
    }
}
