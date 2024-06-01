namespace TransactionalBox.CustomerRegistrations.Messages
{
    internal sealed class CreateCustomerCommandMessageDefinition : OutboxMessageDefinition<CreateCustomerCommandMessage>
    {
        public CreateCustomerCommandMessageDefinition() 
        {
            Receiver = "Customers";
        }
    }
}
