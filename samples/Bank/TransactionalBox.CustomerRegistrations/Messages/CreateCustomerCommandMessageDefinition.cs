namespace TransactionalBox.CustomerRegistrations.Messages
{
    internal sealed class CreateCustomerCommandMessageDefinition : OutboxDefinition<CreateCustomerCommandMessage>
    {
        public CreateCustomerCommandMessageDefinition() 
        {
            Receiver = "Customers";
        }
    }
}
