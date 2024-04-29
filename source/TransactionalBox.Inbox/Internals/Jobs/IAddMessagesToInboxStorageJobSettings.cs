namespace TransactionalBox.Inbox.Internals.Jobs
{
    internal interface IAddMessagesToInboxStorageJobSettings
    {
        TimeSpan DefaultTimeToLiveIdempotencyKey { get; }
    }
}