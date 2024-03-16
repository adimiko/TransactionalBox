namespace TransactionalBox.BackgroundServiceBase.Internals.Context
{
    public interface IJobIdExecutionContextConstructor
    {
        string JobId { set; }
    }
}
