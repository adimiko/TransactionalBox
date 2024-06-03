namespace TransactionalBox.Sample.WebApi.InboxMessages
{
    public class PublishableMessage : InboxMessage
    {
        public string Name { get; init; }

        public int Age { get; init; }
    }
}
