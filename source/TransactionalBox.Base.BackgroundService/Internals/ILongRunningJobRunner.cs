namespace TransactionalBox.Base.BackgroundService.Internals
{
    internal interface ILongRunningJobRunner
    {
        Task<IEnumerable<Task>> Run(Type jobType, int numberOfInstances, CancellationToken stoppingToken);
    }
}
