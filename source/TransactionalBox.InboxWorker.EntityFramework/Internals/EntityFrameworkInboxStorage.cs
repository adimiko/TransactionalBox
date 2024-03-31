using Microsoft.EntityFrameworkCore;
using System.Data;
using TransactionalBox.InboxBase.StorageModel.Internals;
using TransactionalBox.InboxWorker.Internals;
using TransactionalBox.InboxWorker.Internals.Contracts;
using TransactionalBox.Internals;

namespace TransactionalBox.InboxWorker.EntityFramework.Internals
{
    internal sealed class EntityFrameworkInboxStorage : IInboxStorage
    {
        private const IsolationLevel _isolationLevel = IsolationLevel.ReadCommitted;

        private readonly DbContext _dbContext;

        private readonly DbSet<InboxMessage> _inboxMessages;

        private readonly DbSet<IdempotentInboxKey> _idempotentInboxekeys;

        public EntityFrameworkInboxStorage(DbContext dbContext)
        {
            _dbContext = dbContext;
            _inboxMessages = dbContext.Set<InboxMessage>();
            _idempotentInboxekeys = _dbContext.Set<IdempotentInboxKey>();
        }

        public async Task<IEnumerable<IdempotentInboxKey>> GetExistIdempotentInboxKeysBasedOn(IEnumerable<InboxMessage> messages)
        {
            //TODO input ready id
            var ids = messages.Select(x => x.Id);

            var idempotentInboxKeys = await _idempotentInboxekeys
                        .AsNoTracking()
                        .Where(x => ids.Contains(x.Id))
                        .ToListAsync();

            return idempotentInboxKeys;
        }

        public async Task<AddRangeToInboxStorageResult> AddRange(IEnumerable<InboxMessage> messages, DateTime nowUtc) //TODO maybe Result.Success or Failure(sqlProblem or duplicate message)
        {

            var duplicatedInboxKeys = new List<DuplicatedInboxKey>();

            //TODO result with duplicated messages and log id in inbox-Worker

            //TODO create models with created AddedUtc
            foreach (var message in messages) 
            {
                message.AddedUtc = nowUtc;
            }

            var idempotentMessages = messages.Select(x => IdempotentInboxKey.CreateBasedOnInboxMessage(x));

            try
            {
                await _idempotentInboxekeys.AddRangeAsync(idempotentMessages);

                await _inboxMessages.AddRangeAsync(messages);

                await _dbContext.SaveChangesAsync();

                return AddRangeToInboxStorageResult.Success;
            }
            //TODO Add a better check
            catch (DbUpdateException dbUpdateException) when (dbUpdateException.InnerException != null && dbUpdateException.InnerException.Message.Contains("duplicate key"))
            {
                return AddRangeToInboxStorageResult.Failure;
            }
        }

        public async Task<int> RemoveProcessedMessages(int batchSize)
        {
            int rowCount = 0;

            using (var transaction = await _dbContext.Database.BeginTransactionAsync(_isolationLevel))
            {
                rowCount = await _inboxMessages
                    .Where(x => x.IsProcessed)
                    .Take(batchSize)
                    .ExecuteDeleteAsync();

                await transaction.CommitAsync();
            }

            return rowCount;
        }
    }
}
