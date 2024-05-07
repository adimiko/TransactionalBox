namespace TransactionalBox.Base.BackgroundService.Internals.Launchers.Loggers
{
    internal interface ILauncherLogger<T> where T : class
    {
        void UnexpectedError(Exception exception);
    }
}
