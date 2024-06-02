namespace TransactionalBox.Loans.Messages
{
    public class CreatedCustomerEventMessage : InboxMessage
    {
        public Guid Id { get; init; }
    }
}
