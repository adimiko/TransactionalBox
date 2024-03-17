namespace TransactionalBox.BackgroundServiceBase.Internals
{
    public abstract class Job
    {
        protected internal abstract Task Execute(CancellationToken stoppingToken);
    }
}
