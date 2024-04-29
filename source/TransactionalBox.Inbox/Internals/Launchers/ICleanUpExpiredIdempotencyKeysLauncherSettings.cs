namespace TransactionalBox.Inbox.Internals.Launchers
{
    internal interface ICleanUpExpiredIdempotencyKeysLauncherSettings
    {
        int NumberOfInstances { get; }
    }
}
