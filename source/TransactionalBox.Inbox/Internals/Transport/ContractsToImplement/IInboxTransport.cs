namespace TransactionalBox.Inbox.Internals.Transport.ContractsToImplement
{
    internal interface IInboxTransport
    {
        IAsyncEnumerable<TransportMessage> GetMessages(IEnumerable<string> topics, CancellationToken cancellationToken);
    }
}
