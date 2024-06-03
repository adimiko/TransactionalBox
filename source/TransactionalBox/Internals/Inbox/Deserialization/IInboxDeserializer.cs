namespace TransactionalBox.Internals.Inbox.Deserialization
{
    internal interface IInboxDeserializer
    {
        InboxMessage DeserializeMessage(string message, Type type);

        Metadata DeserializeMetadata(string metadata);
    }
}
