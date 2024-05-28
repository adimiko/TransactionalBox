using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using System.Text;
using TransactionalBox.Inbox.Internals.Transport;
using TransactionalBox.Inbox.Internals.Transport.ContractsToImplement;

namespace TransactionalBox.Inbox.RabbitMQ.Internals.ImplementedContracts
{
    internal sealed class RabbitMqInboxTransport : IInboxTransport
    {
        private readonly ConnectionFactory _connectionFactory;

        public RabbitMqInboxTransport(ConnectionFactory connectionFactory) 
        { 
            _connectionFactory = connectionFactory;
        }

        public async IAsyncEnumerable<TransportMessage> GetMessages(IEnumerable<string> topics, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            var queueName = channel.QueueDeclare().QueueName;

            foreach (var bindingKey in topics)
            {
                channel.QueueBind(queue: queueName,
                                  exchange: "topic_logs",
                                  routingKey: bindingKey);
            }

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (sender, ea) =>
            {
                try
                {
                    var x = (string)ea.BasicProperties.Headers["Compression"];

                    var transportMessage = new TransportMessage()
                    {
                        Payload = ea.Body.ToArray(),
                        Compression = x,
                    };

                    //yield return transportMessage;

                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch
                {
                    channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, true);

                    throw;
                }
            };

            await Task.CompletedTask;
        }
    }
}
