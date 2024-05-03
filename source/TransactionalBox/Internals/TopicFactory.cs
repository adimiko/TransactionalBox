namespace TransactionalBox.Internals
{
    internal sealed class TopicFactory : ITopicFactory
    {
        private const char _separator = '.';

        public string Create(string serviceName, string messageName)
        {
            var topic = serviceName + _separator + messageName;

            return topic;
        }
    }
}
