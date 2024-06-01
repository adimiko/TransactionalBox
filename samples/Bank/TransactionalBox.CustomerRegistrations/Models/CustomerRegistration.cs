namespace TransactionalBox.CustomerRegistrations.Models
{
    public class CustomerRegistration
    {
        public required Guid Id { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required int Age { get; set; }

        public bool IsApproved { get; set; } = false;

        public required DateTime CreatedAtUtc { get; init; }

        public required DateTime UpdatedAtUtc { get; init; }
    }
}
