using TransactionalBox.Inbox;
using TransactionalBox.Inbox.Contexts;

namespace TransactionalBox.Sample.InboxWithWorker
{
    internal sealed class PublishableMessageHandler : IInboxMessageHandler<PublishableMessage>
    {
        private readonly ServiceWithInboxDbContext _context;

        public PublishableMessageHandler(ServiceWithInboxDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task Handle(PublishableMessage message, IExecutionContext executionContext)
        {
            //Logic
            await _context.SaveChangesAsync();
        }
    }
}
