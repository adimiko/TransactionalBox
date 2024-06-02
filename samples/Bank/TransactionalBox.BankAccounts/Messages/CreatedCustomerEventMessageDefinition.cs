namespace TransactionalBox.BankAccounts.Messages
{
    public class CreatedCustomerEventMessageDefinition : InboxMessageDefinition<CreatedCustomerEventMessage>
    {
        public CreatedCustomerEventMessageDefinition() 
        {
            PublishedBy = "Customers";
        }
    }
}
