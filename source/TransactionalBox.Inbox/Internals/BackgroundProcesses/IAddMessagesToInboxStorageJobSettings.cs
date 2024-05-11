namespace TransactionalBox.Inbox.Internals.BackgroundProcesses
{
    internal interface IAddMessagesToInboxStorageJobSettings
    {
        TimeSpan DefaultTimeToLiveIdempotencyKey { get; }
    }
}