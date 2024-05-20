using TransactionalBox.Inbox;

namespace TransactionalBox.Sample.WebApi.InMemory.ServiceWithInbox
{
    public sealed class PublishableMessageDefinition : InboxMessageDefinition<PublishableMessage>
    {
        public PublishableMessageDefinition() 
        {
            PublishedBy = "ExampleServiceId";
        }
    }
}
