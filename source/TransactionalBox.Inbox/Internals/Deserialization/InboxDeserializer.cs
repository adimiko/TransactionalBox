using System.Text.Json;

namespace TransactionalBox.Inbox.Internals.Deserialization
{
    internal sealed class InboxDeserializer : IInboxDeserializer
    {
        public InboxMessage DeserializeMessage(string message, Type type)
        {
            return JsonSerializer.Deserialize(message, type) as InboxMessage;
        }

        public Metadata DeserializeMetadata(string metadata)
        {
            return JsonSerializer.Deserialize<Metadata>(metadata);
        }
    }
}
