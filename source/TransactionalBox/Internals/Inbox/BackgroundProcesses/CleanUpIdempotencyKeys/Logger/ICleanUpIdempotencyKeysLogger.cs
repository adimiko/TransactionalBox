using TransactionalBox.Internals.Inbox.BackgroundProcesses.Base.Logger;

namespace TransactionalBox.Internals.Inbox.BackgroundProcesses.CleanUpIdempotencyKeys.Logger
{
    internal interface ICleanUpIdempotencyKeysLogger : IBackgroundProcessBaseLogger
    {
        void CleanedUp(string name, Guid id, long iteration, int numberOfMessages);
    }
}
