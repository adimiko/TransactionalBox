namespace TransactionalBox.CustomerRegistrations.Requests
{
    public sealed class CreateCustomerRegistrationRequest
    {
        public string FirstName { get; init; }

        public string LastName { get; init; }

        public int Age { get; init; }
    }
}
