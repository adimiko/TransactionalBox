namespace TransactionalBox.BackgroundServiceBase.Internals
{
    internal interface IParallelExecutor
    {
        IEnumerable<Task> Run(Type jobType, int numberOfInstances, CancellationToken stoppingToken);
    }
}
