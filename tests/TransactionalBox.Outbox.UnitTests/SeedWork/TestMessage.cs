namespace TransactionalBox.Outbox.UnitTests.SeedWork
{
    public sealed class TestMessage : IOutboxMessage
    {
        public string Name { get; init; } = "Name";
    }
}
