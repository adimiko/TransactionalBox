namespace TransactionalBox.Loans.Messages
{
    public class CreatedCustomerEventMessageDefinition : InboxMessageDefinition<CreatedCustomerEventMessage>
    {
        public CreatedCustomerEventMessageDefinition()
        {
            PublishedBy = "Customers";
        }
    }
}
