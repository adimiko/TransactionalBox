namespace TransactionalBox.Internals.Inbox.Transport.ContractsToImplement
{
    internal interface IInboxTransport
    {
        IAsyncEnumerable<TransportMessage> GetMessages(IEnumerable<string> topics, CancellationToken cancellationToken);
    }
}
