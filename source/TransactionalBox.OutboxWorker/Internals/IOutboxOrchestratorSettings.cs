namespace TransactionalBox.OutboxWorker.Internals
{
    internal interface IOutboxOrchestratorSettings
    {
        int NumberOfOutboxProcessor { get; }
    }
}
