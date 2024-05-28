using TransactionalBox.Inbox.Contexts;

namespace TransactionalBox.End2EndTests.SeedWork.Inbox
{
    internal sealed class SendableMessageInboxHandler : IInboxHandler<SendableMessage>
    {
        private readonly InboxVerifier _inboxVerifier;

        public SendableMessageInboxHandler(InboxVerifier inboxVerifier) 
        {
            _inboxVerifier = inboxVerifier;
        }

        public Task Handle(SendableMessage message, IExecutionContext executionContext)
        {
            _inboxVerifier.IsExecuted = true;

            return Task.CompletedTask;
        }
    }
}
