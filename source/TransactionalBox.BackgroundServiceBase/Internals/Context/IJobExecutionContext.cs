using TransactionalBox.BackgroundServiceBase.Internals.ValueObjects;

namespace TransactionalBox.BackgroundServiceBase.Internals.Context
{
    public interface IJobExecutionContext
    {
        JobId JobId { get; }

        string JobExecutorId { get; }

        string JobName { get; }
    }
}
