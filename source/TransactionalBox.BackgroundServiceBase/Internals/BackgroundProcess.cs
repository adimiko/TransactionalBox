namespace TransactionalBox.BackgroundServiceBase.Internals
{
    public abstract class BackgroundProcess
    {
        protected internal abstract Task Execute(string processId, CancellationToken stoppingToken);
    }
}
