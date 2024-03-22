namespace TransactionalBox.BackgroundServiceBase.Internals
{
    internal interface IParallelExecutor
    {
        Task<IEnumerable<Task>> Run(Type jobType, int numberOfInstances, CancellationToken stoppingToken);
    }
}
