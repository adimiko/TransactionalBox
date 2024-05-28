using RabbitMQ.Client;

namespace TransactionalBox.Outbox.RabbitMQ.Settings
{
    public sealed class OutboxRabbitMqSettings
    {
        public ConnectionFactory ConnectionFactory { get; } = new ConnectionFactory();

        internal OutboxRabbitMqSettings() { }
    }
}
