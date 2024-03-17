namespace TransactionalBox.BackgroundServiceBase.Internals.Context
{
    public interface IJobExecutionContextConstructor
    {
        string JobId { set; }

        string JobExecutiorId { set; }
    }
}
