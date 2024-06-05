namespace TransactionalBox.Sample.WebApi.InboxMessages
{
    public sealed class PublishableMessageDefinition : InboxDefinition<PublishableMessage>
    {
        public PublishableMessageDefinition()
        {
            PublishedBy = "ExampleServiceId";
        }
    }
}
