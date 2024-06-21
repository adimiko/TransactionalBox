using Microsoft.EntityFrameworkCore;
using System.Data;
using TransactionalBox.Internals.Inbox.Storage;
using TransactionalBox.Internals.Inbox.Storage.ContractsToImplement;

namespace TransactionalBox.EntityFrameworkCore.Internals.Inbox.ImplementedContracts
{
    internal sealed class EntityFrameworkCoreCleanUpInboxRepository : ICleanUpInboxRepository
    {
        private const IsolationLevel _isolationLevel = IsolationLevel.ReadCommitted;

        private readonly DbContext _dbContext;

        private readonly DbSet<InboxMessageStorage> _inboxMessages;

        public EntityFrameworkCoreCleanUpInboxRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _inboxMessages = dbContext.Set<InboxMessageStorage>();
        }

        public async Task<int> RemoveProcessedMessages(int batchSize)
        {
            int numberOfDeletedRows;

            using (var transaction = await _dbContext.Database.BeginTransactionAsync(_isolationLevel).ConfigureAwait(false))
            {
                numberOfDeletedRows = await _inboxMessages
                    .Where(x => x.IsProcessed)
                    .Take(batchSize)
                    .ExecuteDeleteAsync()
                    .ConfigureAwait(false);

                await transaction.CommitAsync().ConfigureAwait(false);
            }

            return numberOfDeletedRows;
        }
    }
}
