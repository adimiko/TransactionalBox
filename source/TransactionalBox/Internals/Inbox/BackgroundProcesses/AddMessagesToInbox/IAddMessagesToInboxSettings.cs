namespace TransactionalBox.Internals.Inbox.BackgroundProcesses.AddMessagesToInbox
{
    internal interface IAddMessagesToInboxSettings
    {
        TimeSpan DefaultTimeToLiveIdempotencyKey { get; }
    }
}