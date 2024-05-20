using TransactionalBox.Outbox;

namespace TransactionalBox.Sample.WebApi.OutboxMessages
{
    internal sealed class ExampleMessageDefinition : OutboxMessageDefinition<ExampleMessage>
    {
        public ExampleMessageDefinition() 
        {
            Receiver = "Registrations";
        }
    }
}
