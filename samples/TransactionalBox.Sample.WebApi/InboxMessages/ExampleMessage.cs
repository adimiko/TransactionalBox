using TransactionalBox.Inbox;

namespace TransactionalBox.Sample.WebApi.InboxMessages
{
    public sealed class ExampleMessage : IInboxMessage
    {
        public string Name { get; init; }

        public int Age { get; init; }
    }
}
