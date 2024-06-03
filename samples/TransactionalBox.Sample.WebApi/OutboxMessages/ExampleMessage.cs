namespace TransactionalBox.Sample.WebApi.OutboxMessages
{
    public sealed class ExampleMessage : OutboxMessage
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
