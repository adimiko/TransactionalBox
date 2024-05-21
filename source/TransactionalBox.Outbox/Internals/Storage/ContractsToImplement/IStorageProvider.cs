namespace TransactionalBox.Outbox.Internals.Storage.ContractsToImplement
{
    internal interface IStorageProvider
    {
        string? ProviderName { get; }
    }
}
