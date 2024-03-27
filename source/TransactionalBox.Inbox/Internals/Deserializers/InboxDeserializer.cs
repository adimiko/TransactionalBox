using System.Text.Json;
using TransactionalBox.Inbox.Deserialization;

namespace TransactionalBox.Inbox.Internals.Deserializers
{
    internal sealed class InboxDeserializer : IInboxDeserializer
    {
        public IInboxMessage Deserialize(string data, Type type)
        {
            return JsonSerializer.Deserialize(data, type) as IInboxMessage;
        }
    }
}
