namespace TransactionalBox.InboxWorker.Internals.Services
{
    internal interface ITopicsProvider
    {
        IEnumerable<string> Topics { get; }
    }
}
