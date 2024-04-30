namespace TransactionalBox.Inbox.Internals.Launchers.InboxWorker
{
    internal interface ICleanUpExpiredIdempotencyKeysLauncherSettings
    {
        int NumberOfInstances { get; }
    }
}
