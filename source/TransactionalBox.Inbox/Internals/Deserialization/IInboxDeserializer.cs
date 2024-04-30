namespace TransactionalBox.Inbox.Internals.Deserialization
{
    internal interface IInboxDeserializer
    {
        IInboxMessage DeserializeMessage(string message, Type type);

        Metadata DeserializeMetadata(string metadata);
    }
}
