using TransactionalBox.Outbox.Internals.Transport.ContractsToImplement;

namespace TransactionalBox.Outbox.RabbitMQ.Internals.ImplementedContracts
{
    internal sealed class RabbitMqTransportMessageSizeSettings : ITransportMessageSizeSettings
    {
        public int OptimalTransportMessageSize { get; } = 10240; //TODO
    }
}
