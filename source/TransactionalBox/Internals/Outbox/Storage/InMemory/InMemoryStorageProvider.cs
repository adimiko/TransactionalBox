using TransactionalBox.Internals.Outbox.Storage.ContractsToImplement;

namespace TransactionalBox.Internals.Outbox.Storage.InMemory
{
    internal sealed class InMemoryStorageProvider : IStorageProvider
    {
        public string? ProviderName { get; } = "InMemory";
    }
}
