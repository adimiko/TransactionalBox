using TransactionalBox.Inbox;

namespace TransactionalBox.Sample.InboxWithWorker
{
    internal sealed class ExampleMessageHandler : IInboxMessageHandler<ExampleMessage>
    {
        private readonly ServiceWithInboxDbContext _context;

        public ExampleMessageHandler(ServiceWithInboxDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task Handle(ExampleMessage message, CancellationToken cancellationToken)
        {
            // Logic
            // TODO config AutoSaveChanges = false (default)
            await _context.SaveChangesAsync();
        }
    }
}
