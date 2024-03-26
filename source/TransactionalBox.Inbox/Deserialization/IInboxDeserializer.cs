namespace TransactionalBox.Inbox.Deserialization
{
    public interface IInboxDeserializer
    {
        IInboxMessage Deserialize(string data, Type type);
    }
}
