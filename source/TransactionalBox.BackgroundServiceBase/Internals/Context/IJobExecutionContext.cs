using TransactionalBox.BackgroundServiceBase.Internals.ValueObjects;

namespace TransactionalBox.BackgroundServiceBase.Internals.Context
{
    public interface IJobExecutionContext
    {
        JobId JobId { get; }

        JobExecutorId JobExecutorId { get; }

        JobName JobName { get; }
    }
}
