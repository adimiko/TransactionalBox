namespace TransactionalBox.Inbox.Internals.Topics
{
    internal interface ITopicsProvider
    {
        IEnumerable<string> Topics { get; }
    }
}
