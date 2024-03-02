namespace TransactionalBox.Outbox.UnitTests.SeedWork
{
    public sealed class TestMessage : MessageBase
    {
        public string Name { get; init; } = "Name";
    }
}
