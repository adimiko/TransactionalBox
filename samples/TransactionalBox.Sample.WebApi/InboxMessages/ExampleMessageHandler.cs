using TransactionalBox.Inbox;
using TransactionalBox.Inbox.Contexts;

namespace TransactionalBox.Sample.WebApi.InboxMessages
{
    internal sealed class ExampleMessageHandler : IInboxMessageHandler<ExampleMessage>
    {
        /*
        private readonly SampleDbContext _context;

        public ExampleMessageHandler(SampleDbContext dbContext) 
        {
            _context = dbContext;
        }
        */

        public async Task Handle(ExampleMessage message, IExecutionContext executionContext)
        {
            // Logic
            // TODO config AutoSaveChanges = false (default)
            //await _context.SaveChangesAsync(executionContext.CancellationToken);
        }
    }
}
