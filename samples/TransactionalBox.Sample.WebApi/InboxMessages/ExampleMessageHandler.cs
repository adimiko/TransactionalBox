﻿using TransactionalBox.Inbox;

namespace TransactionalBox.Sample.WebApi.InboxMessages
{
    internal sealed class ExampleMessageHandler : IInboxMessageHandler<ExampleMessage>
    {
        private readonly SampleDbContext _context;

        public ExampleMessageHandler(SampleDbContext dbContext) 
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
