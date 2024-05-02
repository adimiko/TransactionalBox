namespace TransactionalBox.Inbox.Internals
{
    internal sealed class InboxMessageTypes : IInboxMessageTypes
    {
        private readonly Dictionary<string, Type> _dictionaryMessageTypes = new Dictionary<string, Type>();

        private readonly HashSet<Type> _messageTypes = new HashSet<Type>();

        public IReadOnlyDictionary<string, Type> DictionaryMessageTypes => _dictionaryMessageTypes;

        public IEnumerable<Type> MessageTypes => _messageTypes;

        internal InboxMessageTypes(IEnumerable<Type> inboxMessageHandlerTypes, Type handlerGenericType)
        {
            foreach (Type type in inboxMessageHandlerTypes)
            {
                var handlerTypes = type
                    .GetInterfaces()
                    .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == handlerGenericType);

                foreach (Type handlerType in handlerTypes)
                {
                    var messageType = handlerType.GetGenericArguments()[0];

                    _dictionaryMessageTypes.Add(messageType.Name, messageType);
                    _messageTypes.Add(messageType);
                }
            }
        }
    }
}
