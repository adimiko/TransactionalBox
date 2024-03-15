namespace TransactionalBox.BackgroundServiceBase.Internals
{
    //TODO internal
    public interface IParallelExecutor
    {
        IEnumerable<Task> Run(Type jobType, int numberOfInstances, CancellationToken stoppingToken);
    }
}
