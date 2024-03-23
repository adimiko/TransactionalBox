using Microsoft.EntityFrameworkCore;
using System.Data;

namespace TransactionalBox.DistributedLock.EntityFramework.Internals
{
    internal sealed class EntityFrameworkDistributedLock<T> : IDistributedLock<T>
        where T : Lock, new()
    {
        private readonly DbContext _dbContext;

        private readonly DbSet<T> _lockStorage;

        public EntityFrameworkDistributedLock(DbContext dbContext)
        {
            _dbContext = dbContext;
            _lockStorage = dbContext.Set<T>();
        }

        // TODO log when lock is acquire and released (Distributed lock)
        // TODO InMemory lock SemaphoreSlim(1,1)
        // TODO log when lock is WaitOne and Released SemaphoreSlim(1,1)
        private T? _newLock = null;

        private TimeSpan _timeout = TimeSpan.FromMinutes(15);

        public async Task Acquire(string key)
        {

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

                    var previousLock = await _lockStorage
                        .AsNoTracking()
                        .SingleOrDefaultAsync(x => x.Key == key && (x.IsReleased || x.TimeoutUtc <= now));

                    doesPreviousLockExist = previousLock is not null;

                    if (doesPreviousLockExist)
                    {
                        _newLock = previousLock.CreateNewLock<T>(now, _timeout);

                        rowCount = await _lockStorage
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

            var rowCount = 0;

            using (var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted))
            {
                rowCount = await _lockStorage
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
            // SemaphoreSlim(1,1)
            if (!await _lockStorage.AnyAsync(x => x.Key == key))
            {
                try
                {
                    var @lock = Lock.CreateFirstLock<T>(key, DateTime.UtcNow, DateTime.UtcNow); //TODO only key

                    await _lockStorage.AddAsync(@lock);
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
