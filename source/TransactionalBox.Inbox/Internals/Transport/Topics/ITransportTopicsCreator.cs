namespace TransactionalBox.Inbox.Internals.Transport.Topics
{
    internal interface ITransportTopicsCreator
    {
        Task Create(IEnumerable<string> topics);
    }
}
