namespace TransactionalBox.Sample.WebApi.OutboxMessages
{
    internal sealed class ExampleMessageDefinition : OutboxDefinition<ExampleMessage>
    {
        public ExampleMessageDefinition() 
        {
            Receiver = "Registrations";
        }
    }
}
