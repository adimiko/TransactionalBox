using Microsoft.EntityFrameworkCore;
using System.Data;
using TransactionalBox.OutboxBase.StorageModel.Internals;

namespace TransactionalBox.OutboxWorker.EntityFramework.Internals
{
    internal sealed class EntityFrameworkOutboxDistributedLockStorage
    {
        private readonly DbContext _dbContext;

        public EntityFrameworkOutboxDistributedLockStorage(DbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        // TODO log when lock is acquire and released (Distributed lock)
        // TODO InMemory lock SemaphoreSlim(1,1)
        // TODO log when lock is WaitOne and Released SemaphoreSlim(1,1)
        private OutboxLock? _newLock = null;

        private TimeSpan _timeout = TimeSpan.FromMinutes(15);

        public async Task Acquire(string key)
        {
            var outboxLockStorage = _dbContext.Set<OutboxLock>();

            //TODO only when first start
            //TODO InMemory lock :)
            await AddOutboxLockIfNotExist(key);

            int rowCount = 0;

            do
            {
                bool doesPreviousLockExist = false; //rename

                using (var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted))
                {
                    var now = DateTime.UtcNow;

                    var previousLock = await outboxLockStorage
                        .AsNoTracking()
                        .SingleOrDefaultAsync(x => x.Key == key && (x.IsReleased || x.TimeoutUtc <= now));

                    doesPreviousLockExist = previousLock is not null;

                    if (doesPreviousLockExist)
                    {
                        _newLock = previousLock.CreateNewLock(now, _timeout);

                        rowCount = await outboxLockStorage
                        .Where(x => x.Key == key && x.ConcurrencyToken == previousLock.ConcurrencyToken)
                        .ExecuteUpdateAsync(x => x
                        .SetProperty(x => x.IsReleased, _newLock.IsReleased)
                        .SetProperty(x => x.StartUtc, _newLock.StartUtc)
                        .SetProperty(x => x.TimeoutUtc, _newLock.TimeoutUtc)
                        .SetProperty(x => x.ConcurrencyToken, _newLock.ConcurrencyToken));
                    }

                    await transaction.CommitAsync();
                }

                if (!doesPreviousLockExist)
                {
                    //TODO (startDate of lock + job avg time) = potentialReleaseDateTime  <- potentialReleaseDateTime - now  = delay
                    await Task.Delay(50);
                }
            }
            while (rowCount < 1);
        }

        public async Task Release()
        {
            _newLock.Release();

            var outboxLockStorage = _dbContext.Set<OutboxLock>();

            var rowCount = 0;

            using (var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted))
            {
                rowCount = await outboxLockStorage
                    .Where(x => x.Key == _newLock.Key && x.ConcurrencyToken == _newLock.ConcurrencyToken)
                    .ExecuteUpdateAsync(x => x.SetProperty(x => x.IsReleased, _newLock.IsReleased));

                await transaction.CommitAsync();
            }

            if (rowCount < 1) 
            {
                //TODO log
            }
        }

        private async Task AddOutboxLockIfNotExist(string key)
        {
            var outboxLockStorage = _dbContext.Set<OutboxLock>();

            // SemaphoreSlim(1,1)
            if (!await outboxLockStorage.AnyAsync(x => x.Key == key))
            {
                try
                {
                    var outboxLock = OutboxLock.CreateFirstLock(key, DateTime.UtcNow, DateTime.UtcNow); //TODO only key

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
