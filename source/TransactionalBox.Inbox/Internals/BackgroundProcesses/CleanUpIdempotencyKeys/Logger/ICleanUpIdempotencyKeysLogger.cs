using TransactionalBox.Inbox.Internals.BackgroundProcesses.Base.Logger;

namespace TransactionalBox.Inbox.Internals.BackgroundProcesses.CleanUpIdempotencyKeys.Logger
{
    internal interface ICleanUpIdempotencyKeysLogger : IBackgroundProcessBaseLogger
    {
        void CleanedUp(string name, Guid id, long iteration, int numberOfMessages);
    }
}
