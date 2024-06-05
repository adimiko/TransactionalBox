namespace TransactionalBox.Sample.OutboxWithWorker
{
    internal sealed class ExampleMessageDefinition : OutboxDefinition<ExampleMessage>
    {
        public ExampleMessageDefinition() 
        {
            Receiver = "ServiceWithInbox";
        }
    }
}
