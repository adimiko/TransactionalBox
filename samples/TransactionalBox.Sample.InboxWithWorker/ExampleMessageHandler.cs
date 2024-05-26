using TransactionalBox.Inbox;
using TransactionalBox.Inbox.Contexts;

namespace TransactionalBox.Sample.InboxWithWorker
{
    internal sealed class ExampleMessageHandler : IInboxHandler<ExampleMessage>
    {
        private readonly ServiceWithInboxDbContext _context;

        public ExampleMessageHandler(ServiceWithInboxDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task Handle(ExampleMessage message, IExecutionContext executionContext)
        {
            // Logic
            // TODO config AutoSaveChanges = false (default)
            await _context.SaveChangesAsync(executionContext.CancellationToken);
        }
    }
}
