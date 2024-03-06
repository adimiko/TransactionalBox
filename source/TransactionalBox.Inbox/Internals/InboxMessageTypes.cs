using System.Reflection;

namespace TransactionalBox.Inbox.Internals
{
    internal sealed class InboxMessageTypes : IInboxMessageTypes
    {
        private readonly Dictionary<string, Type> _types = new Dictionary<string, Type>();

        public IReadOnlyDictionary<string, Type> Types => _types;

        internal InboxMessageTypes(IEnumerable<Type> inboxMessageHandlerTypes)
        {
            foreach (Type type in inboxMessageHandlerTypes)
            {
                //TODO get by interface type
                var messageType = type.GetInterfaces().Single().GetGenericArguments()[0];

                var messageName = messageType.Name;

                _types.Add(messageName, messageType);
            }
        }
    }
}
