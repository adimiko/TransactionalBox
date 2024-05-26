using TransactionalBox.Inbox;
using TransactionalBox.Inbox.Contexts;

namespace TransactionalBox.Sample.WebApi.InboxMessages
{
    internal sealed class PublishableMessageHandler : IInboxHandler<PublishableMessage>
    {
        public Task Handle(PublishableMessage message, IExecutionContext executionContext)
        {
            //Logic
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }
    }
}
