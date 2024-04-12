using Microsoft.EntityFrameworkCore;
using TransactionalBox.DistributedLock.Internals.Contracts;

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

        public async Task<bool> Release<T>(string key, DateTime nowUtc, DateTime expirationUtc)
            where T : Lock, new()
        {
            var updatedRows = await _dbContext.Set<T>()
                .Where(x => x.Key == key && x.ExpirationUtc == expirationUtc)
                .ExecuteUpdateAsync(x => x.SetProperty(x => x.ExpirationUtc, nowUtc));

            return updatedRows > 0;
        }

        public async Task<bool> TryAcquire<T>(string key, DateTime nowUtc, DateTime newExpirationUtc)
            where T : Lock, new()
        {
            var updatedRows = await _dbContext.Set<T>()
            .Where(x => x.Key == key && x.ExpirationUtc <= nowUtc)
            .ExecuteUpdateAsync(x => x.SetProperty(x => x.ExpirationUtc, newExpirationUtc));

            return updatedRows > 0;
        }
    }
}
