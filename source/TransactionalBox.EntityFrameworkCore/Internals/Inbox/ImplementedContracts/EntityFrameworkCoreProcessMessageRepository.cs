﻿using Microsoft.EntityFrameworkCore;
using System.Data;
using TransactionalBox.Internals.Inbox.Storage;
using TransactionalBox.Internals.Inbox.Storage.ContractsToImplement;
using TransactionalBox.Internals.InternalPackages.DistributedLock;

namespace TransactionalBox.EntityFrameworkCore.Internals.Inbox.ImplementedContracts
{
    internal class EntityFrameworkCoreProcessMessageRepository : IProcessMessageRepository
    {
        private readonly DbContext _dbContext;

        private readonly IDistributedLock<InboxDistributedLock> _distributedLock;

        public EntityFrameworkCoreProcessMessageRepository(
            DbContext dbContext,
            IDistributedLock<InboxDistributedLock> distributedLock)
        {
            _dbContext = dbContext;
            _distributedLock = distributedLock;
        }

        public async Task<InboxMessageStorage?> GetMessage(Guid hookId, string hookName, TimeProvider timeProvider, TimeSpan lockTimeout)
        {
            InboxMessageStorage? message = null;

            int rowCount = 0;

            await using (await _distributedLock.Acquire(hookName.ToString(), timeProvider, lockTimeout, TimeSpan.FromMicroseconds(50)).ConfigureAwait(false))
            {
                var nowUtc = timeProvider.GetUtcNow().UtcDateTime;

                using (var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false))
                {
                    rowCount = await _dbContext.Set<InboxMessageStorage>()
                    .OrderBy(x => x.OccurredUtc)
                    .Where(x => !x.IsProcessed && (x.LockUtc == null || x.LockUtc <= nowUtc))
                    .Take(1)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(x => x.LockUtc, nowUtc + lockTimeout)
                        .SetProperty(x => x.JobId, hookId.ToString()))
                    .ConfigureAwait(false);

                    if (rowCount > 0)
                    {
                        message = await _dbContext.Set<InboxMessageStorage>()
                        .Where(x => !x.IsProcessed && x.JobId == hookId.ToString())
                        .OrderBy(x => x.OccurredUtc)
                        .FirstOrDefaultAsync()
                        .ConfigureAwait(false);
                    }

                    await transaction.CommitAsync().ConfigureAwait(false);
                }
            }

            if (message is not null)
            {
                message.IsProcessed = true;
                message.LockUtc = null;
                message.JobId = null;
            }

            return message;
        }
    }
}
