namespace TransactionalBox.Inbox.Internals.Transport.Topics
{
    internal interface ITopicsProvider
    {
        IEnumerable<string> Topics { get; }
    }
}
