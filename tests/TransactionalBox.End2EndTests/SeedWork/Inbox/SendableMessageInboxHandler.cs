using TransactionalBox.Inbox.Contexts;

namespace TransactionalBox.End2EndTests.SeedWork.Inbox
{
    internal sealed class SendableMessageInboxHandler : IInboxHandler<SendableMessage>
    {
        public Task Handle(SendableMessage message, IExecutionContext executionContext)
        {
            return Task.CompletedTask;
        }
    }
}
