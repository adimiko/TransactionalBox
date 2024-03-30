namespace TransactionalBox.Outbox.Internals
{
    internal sealed class TopicFactory
    {
        private const char _separator = '.';

        public string Create<TMessage>(string serviceName, TMessage message) where TMessage : IOutboxMessage
        {
            var messageType = message.GetType().Name;

            var topic = serviceName + _separator + messageType;

            return topic;
        }
    }
}
