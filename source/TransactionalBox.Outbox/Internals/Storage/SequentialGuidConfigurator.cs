using TransactionalBox.Internals.SequentialGuid;
using TransactionalBox.Outbox.Internals.Storage.ContractsToImplement;

namespace TransactionalBox.Outbox.Internals.Storage
{
    internal sealed class SequentialGuidConfigurator
    {
        private readonly IStorageProvider _storageProvider;

        public SequentialGuidConfigurator(IStorageProvider storageProvider) 
        {
            _storageProvider = storageProvider;
        }


        public SequentialGuidType Create()
        {
            var providerName = _storageProvider.ProviderName;

            return providerName switch
            {
                // TODO Check provider name
                // SequentialAtEnd SQL Server
                // SequentialAsBinar Oracle
                _ => SequentialGuidType.SequentialAsString, // MySQL, PostgreSQL
            };
        }
    }
}
