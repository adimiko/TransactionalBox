namespace TransactionalBox.Sample.WebApi.InMemory.ServiceWithOutbox
{
    public sealed class ExampleMessageDefinition : OutboxDefinition<ExampleMessage>
    {
        public ExampleMessageDefinition() 
        {
            Receiver = "ExampleServiceId";
        } 
    }
}
