using Microsoft.EntityFrameworkCore;
using System.Data;
using TransactionalBox.OutboxBase.StorageModel;

namespace TransactionalBox.OutboxWorker.EntityFramework.Internals
{
    internal sealed class EntityFrameworkOutboxLockStorage
    {
        private readonly DbContext _dbContext;

        public EntityFrameworkOutboxLockStorage(DbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        // TODO log when lock is acquire and released (Distributed lock)
        // TODO InMemory lock SemaphoreSlim(1,1)
        // TODO log when lock is WaitOne and Released SemaphoreSlim(1,1)
        private OutboxLock? _outboxLock = null;

        private TimeSpan _timeout = TimeSpan.FromMinutes(15);

        public async Task<EntityFrameworkOutboxLockStorage> Acquire(string jobExecutorId)
        {
            var outboxLockStorage = _dbContext.Set<OutboxLock>();

            //TODO only when first start
            //TODO InMemory lock :)
            await AddOutboxLockIfNotExist();

            int rowCount = 0;

            do
            {
                using (var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted))
                {
                    var now = DateTime.UtcNow;

                    _outboxLock = await outboxLockStorage.SingleOrDefaultAsync(x =>  x.IsReleased || x.ExpirationUtc <= now || x.JobExecutorId == jobExecutorId);

                    if (_outboxLock is not null)
                    {
                        var lastConcurencyToken = _outboxLock.ConcurrencyToken;

                        _outboxLock.ConcurrencyToken++;

                        rowCount = await outboxLockStorage
                        .Where(x => x.ConcurrencyToken == lastConcurencyToken)
                        .ExecuteUpdateAsync(x => x
                        .SetProperty(x => x.IsReleased, false)
                        .SetProperty(x => x.ExpirationUtc, now + _timeout)
                        .SetProperty(x => x.JobExecutorId, jobExecutorId)
                        .SetProperty(x => x.ConcurrencyToken, _outboxLock.ConcurrencyToken));
                    }

                    await transaction.CommitAsync();
                }

                if (_outboxLock is null)
                {
                    await Task.Delay(250);
                }
            }
            while (rowCount < 1);

            return this;
        }

        public async Task Release()
        {
            var outboxLockStorage = _dbContext.Set<OutboxLock>();

            var rowCount = 0;

            using (var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted))
            {
                rowCount = await outboxLockStorage
                    .Where(x => x.ConcurrencyToken == _outboxLock.ConcurrencyToken)
                    .ExecuteUpdateAsync(x => x.SetProperty(x => x.IsReleased, true));

                transaction.Commit();
            }

            if (rowCount < 1) 
            {
                //TODO log
            }
        }

        private async Task AddOutboxLockIfNotExist()
        {
            var outboxLockStorage = _dbContext.Set<OutboxLock>();

            if (!await outboxLockStorage.AnyAsync())
            {
                try
                {
                    var outboxLock = new OutboxLock()
                    {
                        Id = "OutboxLock",
                        ExpirationUtc = DateTime.UtcNow, //TODO
                        ConcurrencyToken = -9999999999999999, //Problem with long.MinValue
                        IsReleased = true,
                    };

                    await outboxLockStorage.AddAsync(outboxLock);
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    //TODO
                }
            }
        }
    }
}
