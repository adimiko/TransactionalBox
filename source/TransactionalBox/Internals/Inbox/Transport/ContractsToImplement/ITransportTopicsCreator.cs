namespace TransactionalBox.Internals.Inbox.Transport.ContractsToImplement
{
    internal interface ITransportTopicsCreator
    {
        Task Create(IEnumerable<string> topics);
    }
}
