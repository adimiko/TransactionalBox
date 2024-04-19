namespace TransactionalBox.Base.BackgroundService.Internals.Launchers
{
    internal interface ILauncherLogger<T> where T : class
    {
        void UnexpectedError(Exception exception);
    }
}
