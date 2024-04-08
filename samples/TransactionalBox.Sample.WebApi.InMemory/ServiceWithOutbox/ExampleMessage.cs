using TransactionalBox.Outbox;

namespace TransactionalBox.Sample.WebApi.InMemory.ServiceWithOutbox
{
    public sealed class ExampleMessage : IOutboxMessage
    {
        public string Name { get; init; }

        public int Age { get; init; }
    }
}
