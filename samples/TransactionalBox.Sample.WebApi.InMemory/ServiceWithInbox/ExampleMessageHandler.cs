namespace TransactionalBox.Sample.WebApi.InMemory.ServiceWithInbox
{
    internal sealed class ExampleMessageHandler : IInboxHandler<ExampleMessage>
    {
        public async Task Handle(ExampleMessage message, IExecutionContext executionContext)
        {
            // Your Logic
        }
    }
}
