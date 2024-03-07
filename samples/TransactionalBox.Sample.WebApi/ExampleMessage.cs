using TransactionalBox.Outbox;

namespace TransactionalBox.Sample.WebApi
{
    public sealed class ExampleMessage : OutboxMessageBase
    {
        public string Name { get; set; } = "Adrian";

        public int Age { get; set; } = 25;
    }
}
