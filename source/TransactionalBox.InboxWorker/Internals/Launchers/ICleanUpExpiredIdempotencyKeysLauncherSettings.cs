namespace TransactionalBox.InboxWorker.Internals.Launchers
{
    internal interface ICleanUpExpiredIdempotencyKeysLauncherSettings
    {
        int NumberOfInstances { get; }
    }
}
