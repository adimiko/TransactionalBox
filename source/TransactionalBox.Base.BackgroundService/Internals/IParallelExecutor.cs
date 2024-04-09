namespace TransactionalBox.Base.BackgroundService.Internals
{
    internal interface IParallelExecutor
    {
        Task<IEnumerable<Task>> Run(Type jobType, int numberOfInstances, CancellationToken stoppingToken);
    }
}
