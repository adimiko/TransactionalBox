namespace TransactionalBox.Outbox.UnitTests.SeedWork
{
    public sealed class TestMessage : OutboxMessageBase
    {
        public string Name { get; init; } = "Name";
    }
}
