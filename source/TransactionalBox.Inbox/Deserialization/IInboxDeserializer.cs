namespace TransactionalBox.Inbox.Deserialization
{
    public interface IInboxDeserializer
    {
        IInboxMessage DeserializeMessage(string message, Type type);

        Metadata DeserializeMetadata(string metadata);
    }
}
