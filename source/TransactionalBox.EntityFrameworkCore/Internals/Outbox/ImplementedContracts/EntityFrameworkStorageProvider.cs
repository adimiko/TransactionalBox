using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Internals.Storage.ContractsToImplement;

namespace TransactionalBox.EntityFrameworkCore.Internals.Outbox.ImplementedContracts
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
