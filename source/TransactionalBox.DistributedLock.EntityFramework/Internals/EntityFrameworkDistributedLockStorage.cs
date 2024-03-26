using Microsoft.EntityFrameworkCore;
using TransactionalBox.DistributedLock.Internals;

namespace TransactionalBox.DistributedLock.EntityFramework.Internals
{
    internal sealed class EntityFrameworkDistributedLockStorage : IDistributedLockStorage
    {
        private readonly DbContext _dbContext;

        public EntityFrameworkDistributedLockStorage(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddFirstLock<T>(T @lock) 
            where T : Lock, new()
        {
            if (!await _dbContext.Set<T>().AnyAsync(x => x.Key == @lock.Key))
            {
                try
                {
                    await _dbContext.Set<T>().AddAsync(@lock);
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    //TODO log
                }
            }
        }

        public async Task<bool> TryAddNextLock<T>(T @lock, int previousLockConcurrencyToken)
            where T : Lock, new()
        {
            var addedRows = await _dbContext.Set<T>()
                        .Where(x => x.Key == @lock.Key && x.ConcurrencyToken == previousLockConcurrencyToken)
                        .ExecuteUpdateAsync(x => x
                        .SetProperty(x => x.IsReleased, @lock.IsReleased)
                        .SetProperty(x => x.StartUtc, @lock.StartUtc)
                        .SetProperty(x => x.TimeoutUtc, @lock.TimeoutUtc)
                        .SetProperty(x => x.ConcurrencyToken, @lock.ConcurrencyToken));

            return addedRows > 0;
        }

        public Task<T?> GetPreviousReleasedLock<T>(string key, DateTime nowUtc)
            where T : Lock, new()
        {
            return _dbContext.Set<T>().AsNoTracking().SingleOrDefaultAsync(x => x.Key == key && (x.IsReleased || x.TimeoutUtc <= nowUtc));
        }

        public async Task<bool> Release<T>(T @lock)
            where T : Lock, new()
        {
            var updatedRows = await _dbContext.Set<T>()
                    .Where(x => x.Key == @lock.Key && x.ConcurrencyToken == @lock.ConcurrencyToken)
                    .ExecuteUpdateAsync(x => x.SetProperty(x => x.IsReleased, @lock.IsReleased));

            return updatedRows > 0;
        }
    }
}
