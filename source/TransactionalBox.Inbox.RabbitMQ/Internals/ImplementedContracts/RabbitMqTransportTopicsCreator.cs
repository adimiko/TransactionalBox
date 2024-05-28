using TransactionalBox.Inbox.Internals.Transport.ContractsToImplement;

namespace TransactionalBox.Inbox.RabbitMQ.Internals.ImplementedContracts
{
    internal sealed class RabbitMqTransportTopicsCreator : ITransportTopicsCreator
    {
        public Task Create(IEnumerable<string> topics)
        {
            throw new NotImplementedException();
        }
    }
}
