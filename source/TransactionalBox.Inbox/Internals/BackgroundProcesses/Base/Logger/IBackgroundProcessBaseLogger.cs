namespace TransactionalBox.Inbox.Internals.BackgroundProcesses.Base.Logger
{
    internal interface IBackgroundProcessBaseLogger
    {
        void UnexpectedException(Exception exception);
    }
}
