﻿using Microsoft.EntityFrameworkCore;
using System.Data;
using TransactionalBox.BackgroundServiceBase.Internals.Context;
using TransactionalBox.BackgroundServiceBase.Internals.ValueObjects;
using TransactionalBox.OutboxBase.StorageModel;
using TransactionalBox.OutboxWorker.Internals;

namespace TransactionalBox.OutboxWorker.EntityFramework.Internals
{
    internal sealed class EntityFrameworkOutboxStorage : IOutboxStorage
    {
        private readonly DbContext _dbContext;

        private readonly DbSet<OutboxMessage> _outboxMessages;

        private readonly EntityFrameworkOutboxLockStorage _distributedLock;

        private readonly IJobExecutionContext _jobExecutionContext;

        public EntityFrameworkOutboxStorage(
            DbContext dbContext,
            EntityFrameworkOutboxLockStorage frameworkOutboxLockStorage,
            IJobExecutionContext jobExecutionContext) 
        {
            _dbContext = dbContext;
            _outboxMessages = dbContext.Set<OutboxMessage>();
            _distributedLock = frameworkOutboxLockStorage;
            _jobExecutionContext = jobExecutionContext;
        }
        //TODO MarkToProcess
        public async Task<IEnumerable<OutboxMessage>> GetMessages(JobId jobId, int batchSize, DateTime nowUtc, DateTime lockUtc)
        {
            int rowCount = 0;

            await _distributedLock.Acquire(_jobExecutionContext.JobName);

            using (var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted))
            {
                rowCount = await _outboxMessages
                .OrderBy(x => x.OccurredUtc)
                .Where(x => x.ProcessedUtc == null && (x.LockUtc == null || x.LockUtc <= nowUtc))
                .Take(batchSize)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.LockUtc, lockUtc)
                    .SetProperty(x => x.JobId, jobId.ToString()));

                await transaction.CommitAsync();
            }

            await _distributedLock.Release();

            if (rowCount < 1)
            {
                return Enumerable.Empty<OutboxMessage>();
            }

            var messages = await _outboxMessages
                .AsNoTracking()
                .Where(x => x.ProcessedUtc == null && x.JobId == jobId.ToString())
                .ToListAsync();

            return messages;
        }

        public async Task MarkAsProcessed(JobId jobId, DateTime processedUtc)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted))
            {
                await _outboxMessages
                    .Where(x => x.ProcessedUtc == null && x.JobId == jobId.ToString())
                    .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.ProcessedUtc, processedUtc));

                await transaction.CommitAsync();
            }
        }
    }
}
