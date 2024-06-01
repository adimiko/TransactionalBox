namespace TransactionalBox.CustomerRegistrations.Messages
{
    internal sealed class CreateCustomerCommandDefinition : OutboxMessageDefinition<CreateCustomerCommand>
    {
        public CreateCustomerCommandDefinition() 
        {
            Receiver = "Customers";
        }
    }
}
