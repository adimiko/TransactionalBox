namespace TransactionalBox.Internals.Outbox.Storage.ContractsToImplement
{
    internal interface IStorageProvider
    {
        string? ProviderName { get; }
    }
}
