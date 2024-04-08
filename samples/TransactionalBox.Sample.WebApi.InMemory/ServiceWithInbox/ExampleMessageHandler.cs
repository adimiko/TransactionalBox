using TransactionalBox.Inbox;
using TransactionalBox.Inbox.Contexts;

namespace TransactionalBox.Sample.WebApi.InMemory.ServiceWithInbox
{
    internal sealed class ExampleMessageHandler : IInboxMessageHandler<ExampleMessage>
    {
        public async Task Handle(ExampleMessage message, IExecutionContext executionContext)
        {
            // Your Logic
        }
    }
}
