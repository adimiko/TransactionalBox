namespace TransactionalBox.Sample.WebApi.InMemory.ServiceWithOutbox
{
    public sealed class ExampleMessageDefinition : OutboxMessageDefinition<ExampleMessage>
    {
        public ExampleMessageDefinition() 
        {
            Receiver = "ExampleServiceId";
        } 
    }
}
