using TransactionalBox.Inbox;
using TransactionalBox.Outbox;

namespace TransactionalBox.Sample.WebApi
{
    public sealed class ExampleMessage : IOutboxMessage, IInboxMessage
    {
        public string Name { get; set; } = "Adrian";

        public int Age { get; set; } = 25;
    }
}
