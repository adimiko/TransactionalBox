using TransactionalBox.Inbox;

namespace TransactionalBox.Sample.WebApi.InMemory.ServiceWithInbox
{
    [PublishedBy("ExampleServiceId")]
    public class PublishableMessage : IInboxMessage
    {
        public string Name { get; init; }

        public int Age { get; init; }
    }
}
