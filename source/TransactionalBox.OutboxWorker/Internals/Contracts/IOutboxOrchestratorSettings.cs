namespace TransactionalBox.OutboxWorker.Internals.Contracts
{
    internal interface IOutboxOrchestratorSettings
    {
        int NumberOfOutboxProcessor { get; }
    }
}
