namespace TransactionalBox.Sample.WebApi.InMemory.ServiceWithOutbox
{
    public sealed class ExampleMessage : OutboxMessage
    {
        public string Name { get; init; }

        public int Age { get; init; }
    }
}
