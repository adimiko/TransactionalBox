namespace TransactionalBox.Inbox.Internals.Settings
{
    internal interface IInboxLauncherSettings
    {
        int NumberOfProcessMessageFromInboxStorageExecutors { get; }
    }
}
