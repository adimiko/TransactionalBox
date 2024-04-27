namespace TransactionalBox.Base.Inbox.MessageTypesFromAssemblies.Internals
{
    internal sealed class InboxMessageTypes : IInboxMessageTypes
    {
        private readonly Dictionary<string, Type> _types = new Dictionary<string, Type>();

        public IReadOnlyDictionary<string, Type> Types => _types;

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

                    _types.Add(messageType.Name, messageType);
                }
            }
        }
    }
}
