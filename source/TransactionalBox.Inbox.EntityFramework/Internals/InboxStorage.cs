using Microsoft.EntityFrameworkCore;
using TransactionalBox.Inbox.Internals;
using TransactionalBox.InboxBase.StorageModel;

namespace TransactionalBox.Inbox.EntityFramework.Internals
{
    internal sealed class InboxStorage : IInboxStorage
    {
        private readonly DbContext _dbContext;

        public InboxStorage(DbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<InboxMessageStorageModel?> GetMessage()
        {
            var message = await _dbContext.Set<InboxMessageStorageModel>()
                .Where(x => !x.IsProcessed)
                .OrderBy(x => x.OccurredUtc)
                .FirstOrDefaultAsync();

            if (message is not null) 
            {
                message.IsProcessed = true;
            }
            
            return message;
        }
    }
}
