namespace TransactionalBox.Internals.Inbox.Transport.Topics
{
    internal interface ITopicsProvider
    {
        IEnumerable<string> Topics { get; }
    }
}
