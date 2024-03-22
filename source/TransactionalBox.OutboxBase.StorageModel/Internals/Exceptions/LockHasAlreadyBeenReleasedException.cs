using TransactionalBox.Internals;

namespace TransactionalBox.OutboxBase.StorageModel.Internals.Exceptions
{
    internal sealed class LockHasAlreadyBeenReleasedException : Exception //TODO own internal type
    {
        internal LockHasAlreadyBeenReleasedException(string key)
            : base("Key: " + key) { }
    }
}
