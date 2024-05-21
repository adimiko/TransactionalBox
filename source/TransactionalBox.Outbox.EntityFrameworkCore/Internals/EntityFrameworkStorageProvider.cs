using Microsoft.EntityFrameworkCore;
using TransactionalBox.Outbox.Internals.Storage.ContractsToImplement;

namespace TransactionalBox.Outbox.EntityFrameworkCore.Internals
{
    internal sealed class EntityFrameworkStorageProvider : IStorageProvider
    {
        public string? ProviderName { get; }

        public EntityFrameworkStorageProvider(DbContext dbContext) 
        {
            ProviderName = dbContext.Database.ProviderName;
        }

        
    }
}
