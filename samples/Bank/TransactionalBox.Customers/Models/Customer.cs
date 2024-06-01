namespace TransactionalBox.Customers.Models
{
    public class Customer
    {
        public Guid Id { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }

        public int Age { get; init; }

        public required DateTime CreatedAtUtc { get; init; }
    }
}
