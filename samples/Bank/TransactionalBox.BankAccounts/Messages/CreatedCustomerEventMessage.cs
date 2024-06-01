namespace TransactionalBox.BankAccounts.Messages
{
    public class CreatedCustomerEventMessage : InboxMessage
    {
        public Guid Id { get; init; }
    }
}
