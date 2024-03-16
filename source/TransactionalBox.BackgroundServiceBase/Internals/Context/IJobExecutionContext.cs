namespace TransactionalBox.BackgroundServiceBase.Internals.Context
{
    public interface IJobExecutionContext
    {
        string JobId { get; }

        string JobExecutiorId { get; }
    }
}
