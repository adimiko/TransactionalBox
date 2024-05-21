using TransactionalBox.Outbox.Internals.Storage.ContractsToImplement;

namespace TransactionalBox.Outbox.Internals.Storage.InMemory
{
    internal sealed class InMemoryStorageProvider : IStorageProvider
    {
        public string? ProviderName { get; } = "InMemory";
    }
}
