namespace TransactionalBox.Outbox.Internals
{
    internal sealed class TopicFactory
    {
        private const char _separator = '-';

        public string Create<TMessage>(string moduleName, TMessage message) where TMessage : MessageBase
        {
            var messageType = message.GetType().Name;

            var topic = moduleName + _separator + messageType;

            return topic;
        }
    }
}
