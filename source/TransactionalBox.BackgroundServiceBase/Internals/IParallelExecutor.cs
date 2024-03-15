namespace TransactionalBox.BackgroundServiceBase.Internals
{
    public interface IParallelExecutor
    {
        IEnumerable<Task> Run<T>(int numberOfInstances, CancellationToken stoppingToken)
            where T : Job;
    }
}
