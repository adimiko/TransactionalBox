using Microsoft.EntityFrameworkCore;
using TransactionalBox.InboxBase.StorageModel;
using TransactionalBox.InboxWorker.Internals;

namespace TransactionalBox.InboxWorker.EntityFramework.Internals
{
    internal sealed class EntityFrameworkInboxStorage : IInboxStorage
    {

        private readonly DbContext _dbContext;

        private readonly DbSet<InboxMessage> _inbox;

        public EntityFrameworkInboxStorage(DbContext dbContext)
        {
            _dbContext = dbContext;
            _inbox = dbContext.Set<InboxMessage>();
        }

        public async Task Add(InboxMessage message)
        {
            await _inbox.AddAsync(message).AsTask();

            await _dbContext.SaveChangesAsync();
        }
    }
}
