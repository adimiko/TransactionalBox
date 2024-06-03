namespace TransactionalBox.Sample.WebApi.InboxMessages
{
    public sealed class ExampleMessage : InboxMessage
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
