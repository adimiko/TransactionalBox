namespace TransactionalBox.CustomerRegistrations.Messages
{
    public sealed class CreateCustomerCommandMessage : OutboxMessage
    {
        public Guid Id { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }

        public int Age { get; init; }
    }
}
