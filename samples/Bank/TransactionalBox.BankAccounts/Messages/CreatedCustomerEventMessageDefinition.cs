namespace TransactionalBox.BankAccounts.Messages
{
    public class CreatedCustomerEventMessageDefinition : InboxDefinition<CreatedCustomerEventMessage>
    {
        public CreatedCustomerEventMessageDefinition() 
        {
            PublishedBy = "Customers";
        }
    }
}
