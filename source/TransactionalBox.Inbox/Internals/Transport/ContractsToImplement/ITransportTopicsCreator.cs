namespace TransactionalBox.Inbox.Internals.Transport.ContractsToImplement
{
    internal interface ITransportTopicsCreator
    {
        Task Create(IEnumerable<string> topics);
    }
}
