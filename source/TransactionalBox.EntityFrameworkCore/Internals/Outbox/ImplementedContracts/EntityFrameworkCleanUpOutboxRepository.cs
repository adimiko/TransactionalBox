using System.Data;
using Microsoft.EntityFrameworkCore;
using TransactionalBox.Outbox.Internals.Storage;
using TransactionalBox.Outbox.Internals.Storage.ContractsToImplement;

namespace TransactionalBox.EntityFrameworkCore.Internals.Outbox.ImplementedContracts
{
    internal sealed class EntityFrameworkCleanUpOutboxRepository : ICleanUpOutboxRepository
    {
        private const IsolationLevel _isolationLevel = IsolationLevel.ReadCommitted;

        private readonly DbContext _dbContext;

        private readonly DbSet<OutboxMessageStorage> _outboxMessages;

        public EntityFrameworkCleanUpOutboxRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _outboxMessages = dbContext.Set<OutboxMessageStorage>();
        }

        public async Task<int> RemoveProcessedMessages(int batchSize)
        {
            int rowCount = 0;

            using (var transaction = await _dbContext.Database.BeginTransactionAsync(_isolationLevel))
            {
                rowCount = await _outboxMessages
                    .Where(x => x.IsProcessed)
                    .OrderBy(x => x.OccurredUtc)
                    .Take(batchSize)
                    .ExecuteDeleteAsync();

                await transaction.CommitAsync();
            }

            return rowCount;
        }
    }
}
