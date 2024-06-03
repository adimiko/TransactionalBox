namespace TransactionalBox.Sample.WebApi.InMemory.ServiceWithInbox
{
    public sealed class ExampleMessage : InboxMessage
    {
        public string Name { get; init; }

        public int Age { get; init; }
    }
}
