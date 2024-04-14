namespace TransactionalBox.InboxWorker.Internals.Jobs
{
    internal interface IAddMessagesToInboxStorageJobSettings
    {
        TimeSpan DefaultTimeToLiveIdempotencyKey { get; }
    }
}