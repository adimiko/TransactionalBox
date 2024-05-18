using Microsoft.EntityFrameworkCore;
using System.Data;
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
            if (!await _dbContext.Set<T>().AnyAsync(x => x.Key == @lock.Key).ConfigureAwait(false))
            {
                try
                {
                    _dbContext.Set<T>().Add(@lock);
                    await _dbContext.SaveChangesAsync().ConfigureAwait(false);
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
            int updatedRows = await _dbContext.Set<T>()
                .Where(x => x.Key == key && x.ExpirationUtc == expirationUtc)
                .ExecuteUpdateAsync(x => x.SetProperty(x => x.ExpirationUtc, nowUtc))
                .ConfigureAwait(false);

            return updatedRows > 0;
        }

        public async Task<bool> TryAcquire<T>(string key, DateTime nowUtc, DateTime newExpirationUtc)
            where T : Lock, new()
        {
            int updatedRows = await _dbContext.Set<T>()
                .Where(x => x.Key == key && x.ExpirationUtc <= nowUtc)
                .ExecuteUpdateAsync(x => x.SetProperty(x => x.ExpirationUtc, newExpirationUtc))
                .ConfigureAwait(false);

            return updatedRows > 0;
        }
    }
}
