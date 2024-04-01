using System.Text.Json;
using TransactionalBox.Inbox.Deserialization;

namespace TransactionalBox.Inbox.Internals.Deserializers
{
    internal sealed class InboxDeserializer : IInboxDeserializer
    {
        public IInboxMessage DeserializeMessage(string message, Type type)
        {
            return JsonSerializer.Deserialize(message, type) as IInboxMessage;
        }

        public Metadata DeserializeMetadata(string metadata)
        {
            return JsonSerializer.Deserialize<Metadata>(metadata);
        }
    }
}
