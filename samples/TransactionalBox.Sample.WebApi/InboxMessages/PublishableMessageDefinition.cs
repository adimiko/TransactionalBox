using TransactionalBox.Inbox;

namespace TransactionalBox.Sample.WebApi.InboxMessages
{
    public sealed class PublishableMessageDefinition : InboxMessageDefinition<PublishableMessage>
    {
        public PublishableMessageDefinition()
        {
            PublishedBy = "ExampleServiceId";
        }
    }
}
