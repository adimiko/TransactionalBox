using Microsoft.EntityFrameworkCore;
using System.Data;
using TransactionalBox.Internals.Inbox.Storage;
using TransactionalBox.Internals.Inbox.Storage.ContractsToImplement;

namespace TransactionalBox.EntityFrameworkCore.Internals.Inbox.ImplementedContracts
{
    internal sealed class EntityFrameworkInboxWorkerStorage : IInboxWorkerStorage
    {
        private const IsolationLevel _isolationLevel = IsolationLevel.ReadCommitted;

        private readonly DbContext _dbContext;

        private readonly DbSet<InboxMessageStorage> _inboxMessages;

        private readonly DbSet<IdempotentInboxKey> _idempotentInboxKeys;

        public EntityFrameworkInboxWorkerStorage(DbContext dbContext)
        {
            _dbContext = dbContext;
            _inboxMessages = dbContext.Set<InboxMessageStorage>();
            _idempotentInboxKeys = _dbContext.Set<IdempotentInboxKey>();
        }

        public async Task<IEnumerable<IdempotentInboxKey>> GetExistIdempotentInboxKeysBasedOn(IEnumerable<InboxMessageStorage> messages)
        {
            //TODO input ready id
            var ids = messages.Select(x => x.Id);

            IEnumerable<IdempotentInboxKey> idempotentInboxKeys;

            using (var transaction = await _dbContext.Database.BeginTransactionAsync(_isolationLevel).ConfigureAwait(false))
            {
                idempotentInboxKeys = await _idempotentInboxKeys
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync()
                .ConfigureAwait(false);

                await transaction.CommitAsync().ConfigureAwait(false);
            }

            return idempotentInboxKeys;
        }

        public async Task<AddRangeToInboxStorageResult> AddRange(IEnumerable<InboxMessageStorage> messages, IEnumerable<IdempotentInboxKey> idempotentInboxKeys)
        {
            try
            {
                await _idempotentInboxKeys.AddRangeAsync(idempotentInboxKeys).ConfigureAwait(false);

                await _inboxMessages.AddRangeAsync(messages).ConfigureAwait(false);

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                return AddRangeToInboxStorageResult.Success;
            }
            //TODO Add a better check
            catch (DbUpdateException dbUpdateException) when (dbUpdateException.InnerException != null && dbUpdateException.InnerException.Message.Contains("duplicate key"))
            {//TODO maybe Result.Success or Failure(sqlProblem or duplicate message)
                return AddRangeToInboxStorageResult.Failure;
            }
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

        public async Task<int> RemoveExpiredIdempotencyKeys(int batchSize, DateTime nowUtc)
        {
            int numberOfDeletedRows;

            using (var transaction = await _dbContext.Database.BeginTransactionAsync(_isolationLevel).ConfigureAwait(false))
            {
                numberOfDeletedRows = await _idempotentInboxKeys
                    .Where(x => x.ExpirationUtc <= nowUtc)
                    .Take(batchSize)
                    .ExecuteDeleteAsync()
                    .ConfigureAwait(false);

                await transaction.CommitAsync().ConfigureAwait(false);
            }

            return numberOfDeletedRows;
        }
    }
}
