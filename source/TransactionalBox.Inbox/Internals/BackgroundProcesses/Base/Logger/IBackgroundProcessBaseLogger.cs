namespace TransactionalBox.Inbox.Internals.BackgroundProcesses.Base.Logger
{
    internal interface IBackgroundProcessBaseLogger
    {
        void UnexpectedException(string name, long attempt, long msDelay, Exception exception);
    }
}
