﻿namespace TransactionalBox.BackgroundServiceBase.Internals.Loggers
{
    internal interface ILauncherLogger<T> where T : class
    {
        void UnexpectedError(Exception exception);
    }
}
