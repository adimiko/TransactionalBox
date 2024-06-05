namespace TransactionalBox.Loans.Messages
{
    public class CreatedCustomerEventMessageDefinition : InboxDefinition<CreatedCustomerEventMessage>
    {
        public CreatedCustomerEventMessageDefinition()
        {
            PublishedBy = "Customers";
        }
    }
}
