using Microsoft.EntityFrameworkCore;
using System.Data;
using TransactionalBox.Base.Inbox.StorageModel.Internals;
using TransactionalBox.InboxWorker.Internals;
using TransactionalBox.InboxWorker.Internals.Contracts;

namespace TransactionalBox.InboxWorker.Storage.EntityFramework.Internals
{
    internal sealed class EntityFrameworkInboxStorage : IInboxWorkerStorage
    {
        private readonly DbContext _dbContext;

        private readonly DbSet<InboxMessage> _inboxMessages;

        private readonly DbSet<IdempotentInboxKey> _idempotentInboxKeys;

        public EntityFrameworkInboxStorage(DbContext dbContext)
        {
            _dbContext = dbContext;
            _inboxMessages = dbContext.Set<InboxMessage>();
            _idempotentInboxKeys = _dbContext.Set<IdempotentInboxKey>();
        }

        public async Task<IEnumerable<IdempotentInboxKey>> GetExistIdempotentInboxKeysBasedOn(IEnumerable<InboxMessage> messages)
        {
            //TODO input ready id
            var ids = messages.Select(x => x.Id);

            var idempotentInboxKeys = await _idempotentInboxKeys
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
                await _idempotentInboxKeys.AddRangeAsync(idempotentMessages);

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

        public Task<int> RemoveProcessedMessages(int batchSize)
        {
            return _inboxMessages
                    .Where(x => x.IsProcessed)
                    .Take(batchSize)
                    .ExecuteDeleteAsync();
        }
    }
}
