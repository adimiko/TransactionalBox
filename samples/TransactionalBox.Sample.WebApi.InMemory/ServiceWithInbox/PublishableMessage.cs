using TransactionalBox.Inbox;

namespace TransactionalBox.Sample.WebApi.InMemory.ServiceWithInbox
{
    public class PublishableMessage : InboxMessage
    {
        public string Name { get; init; }

        public int Age { get; init; }
    }
}
