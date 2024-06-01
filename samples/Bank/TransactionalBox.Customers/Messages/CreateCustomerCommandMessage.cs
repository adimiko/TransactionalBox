namespace TransactionalBox.Customers.Messages
{
    public sealed class CreateCustomerCommandMessage : InboxMessage
    {
        public Guid Id { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }

        public int Age { get; init; }
    }
}
