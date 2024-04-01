namespace TransactionalBox.Inbox.Internals.Jobs
{
    internal interface IProcessMessageFromInboxJobSettings
    {
        TimeSpan DelayWhenInboxIsEmpty { get; }
    }
}
