using System.Text.Json;
using TransactionalBox.Inbox.Deserialization;

namespace TransactionalBox.Inbox.Internals.Deserializers
{
    internal sealed class InboxDeserializer : IInboxDeserializer
    {
        public IInboxMessage DeserializeMessage(string data, Type type)
        {
            return JsonSerializer.Deserialize(data, type) as IInboxMessage;
        }

        public Metadata DeserializeMetadata(string metadata)
        {
            return JsonSerializer.Deserialize<Metadata>(metadata);
        }
    }
}
