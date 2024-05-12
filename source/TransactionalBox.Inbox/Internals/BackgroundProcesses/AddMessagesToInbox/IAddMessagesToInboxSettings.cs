namespace TransactionalBox.Inbox.Internals.BackgroundProcesses.AddMessagesToInbox
{
    internal interface IAddMessagesToInboxSettings
    {
        TimeSpan DefaultTimeToLiveIdempotencyKey { get; }
    }
}