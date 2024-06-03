namespace TransactionalBox.Sample.OutboxWithWorker
{
    internal sealed class ExampleMessageDefinition : OutboxMessageDefinition<ExampleMessage>
    {
        public ExampleMessageDefinition() 
        {
            Receiver = "ServiceWithInbox";
        }
    }
}
