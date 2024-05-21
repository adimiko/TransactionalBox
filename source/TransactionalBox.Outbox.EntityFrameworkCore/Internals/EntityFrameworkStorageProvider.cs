using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Internals.Storage.ContractsToImplement;

namespace TransactionalBox.Outbox.EntityFrameworkCore.Internals
{
    internal sealed class EntityFrameworkStorageProvider : IStorageProvider
    {
        public string? ProviderName { get; }

        public EntityFrameworkStorageProvider(IServiceProvider serviceProvider) 
        {
            using (var scope = serviceProvider.CreateScope()) 
            {
                ProviderName = scope.ServiceProvider.GetRequiredService<DbContext>().Database.ProviderName;
            }
        }

        
    }
}
