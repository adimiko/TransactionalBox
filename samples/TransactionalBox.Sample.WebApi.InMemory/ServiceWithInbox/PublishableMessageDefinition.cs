namespace TransactionalBox.Sample.WebApi.InMemory.ServiceWithInbox
{
    public sealed class PublishableMessageDefinition : InboxDefinition<PublishableMessage>
    {
        public PublishableMessageDefinition() 
        {
            PublishedBy = "ExampleServiceId";
        }
    }
}
