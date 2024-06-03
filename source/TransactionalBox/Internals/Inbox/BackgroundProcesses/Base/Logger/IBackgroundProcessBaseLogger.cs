namespace TransactionalBox.Internals.Inbox.BackgroundProcesses.Base.Logger
{
    internal interface IBackgroundProcessBaseLogger
    {
        void UnexpectedException(string name, long attempt, long msDelay, Exception exception);
    }
}
