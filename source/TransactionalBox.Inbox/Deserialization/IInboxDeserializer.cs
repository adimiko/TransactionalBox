namespace TransactionalBox.Inbox.Deserialization
{
    public interface IInboxDeserializer
    {
        IInboxMessage DeserializeMessage(string data, Type type);

        Metadata DeserializeMetadata(string metadata);
    }
}
