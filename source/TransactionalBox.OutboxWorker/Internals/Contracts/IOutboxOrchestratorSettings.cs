namespace TransactionalBox.OutboxWorker.Internals.Contracts
{
    internal interface IOutboxOrchestratorSettings
    {
        int NumberOfAddMessagesToTransportJobExecutors { get; }
    }
}
