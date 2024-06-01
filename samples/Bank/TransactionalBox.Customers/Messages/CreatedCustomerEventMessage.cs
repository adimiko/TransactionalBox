namespace TransactionalBox.Customers.Messages
{
    public class CreatedCustomerEventMessage : OutboxMessage
    {
        public Guid Id { get; init; }
    }
}
