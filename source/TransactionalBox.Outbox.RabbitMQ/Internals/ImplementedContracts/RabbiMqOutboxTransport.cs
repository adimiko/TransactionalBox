using RabbitMQ.Client;
using TransactionalBox.Outbox.Internals.Hooks.Handlers.AddMessagesToTransport.TransportMessageFactories;
using TransactionalBox.Outbox.Internals.Transport.ContractsToImplement;

namespace TransactionalBox.Outbox.RabbitMQ.Internals.ImplementedContracts
{
    internal sealed class RabbiMqOutboxTransport : IOutboxTransport
    {
        private readonly ConnectionFactory _connectionFactory;

        public RabbiMqOutboxTransport(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Task Add(TransportEnvelope transportEnvelope)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            var exchangeName = transportEnvelope.Topic;

            channel.ExchangeDeclare(exchange: exchangeName,
                type: ExchangeType.Fanout,
                durable: true,
                autoDelete: false,
                arguments: null);

            channel.BasicPublish(exchangeName, exchangeName, basicProperties: null, body: transportEnvelope.Payload);

            return Task.CompletedTask;
        }
    }
}
