namespace TransactionalBox.Inbox.Deserialization
{
    internal interface IInboxDeserializer
    {
        IInboxMessage DeserializeMessage(string message, Type type);

        Metadata DeserializeMetadata(string metadata);
    }
}
